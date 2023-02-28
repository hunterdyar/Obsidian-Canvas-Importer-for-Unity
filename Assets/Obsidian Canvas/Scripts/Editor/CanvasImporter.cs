using System;
using UnityEditor.AssetImporters;
using UnityEngine;
using System.IO;
using System.Linq;
using ObsidianCanvas;
using ObsidianCanvas.Data;
using ObsidianCanvas.JSONTypes;
using UnityEditor;

[ScriptedImporter(1,"canvas")]
public class CanvasImporter : ScriptedImporter
{
	[Tooltip("Handle the following file nodes as TExtAssets in unity. Must be all lowercase. This list must include .md in order for regular obsidian text files to be treated as TextAssets.")]
	public string[] textAssetExtensions = new[] { ".md",".markdown" };
	public override void OnImportAsset(AssetImportContext ctx)
	{
		var canvasObject = ScriptableObject.CreateInstance<CanvasObject>();
		canvasObject.name = Path.GetFileNameWithoutExtension(ctx.assetPath)+" obj";
		
		ImportData(ctx,canvasObject,File.ReadAllText(ctx.assetPath));
		EditorUtility.SetDirty(canvasObject);
		ctx.AddObjectToAsset("Canvas Object",canvasObject);
		ctx.SetMainObject(canvasObject);
		
		//needed?
		AssetDatabase.SaveAssets();
	}

	private void ImportData(AssetImportContext ctx,CanvasObject canvasObject,string canvasText)
	{
		//re-init.
		//Do this before null or empty because empty (ie: just created) files are valid.
		canvasObject.Clear();

		if (string.IsNullOrEmpty(canvasText))
		{
			return;
		}

		//Import to JSON data types, stored in canvasData.
		CanvasData canvasData = JsonUtility.FromJson<CanvasData>(canvasText);

		//populate canvas with "proper" edges and nodes, created from the JSON types.
		foreach (var node in canvasData.nodes)
		{
			var datNode = CreateDataNodeFromJSONDataNode(ctx,node);
			if (datNode != null)//unhandled assets types may be null.
			{
				canvasObject.AddNode(datNode);
			}
		}

		foreach (var edge in canvasData.edges)
		{
			//Defaults are different. The default line is directional, from has none, to has arrow.
			var fromEnd = GetEdgeEnd(edge.fromEnd,EdgeEnd.None);
			var toEnd = GetEdgeEnd(edge.toEnd,EdgeEnd.Arrow);
			
			canvasObject.ConnectNodes(edge.fromNode, GetNodePortSide(edge.fromSide), fromEnd, GetNodePortSide(edge.toSide), edge.toNode, toEnd,edge.color,edge.label);
		}
	}

	private ObsidianCanvas.Data.Node CreateDataNodeFromJSONDataNode(AssetImportContext ctx,ObsidianCanvas.JSONTypes.Node node)
    {
        ObsidianCanvas.Data.Node n = new ObsidianCanvas.Data.Node(node);

    	//Create appropriate subclasses from the single JSON deserialized object type.
    	if (node.type == "text")
    	{
	        var tn = new TextNode(node.text,n);
	        return tn;
        }else if (node.type == "file")
    	{
	        //Handle file type nodes
	        //its better to check the imported type than make assumptions from file extensions.
	        //we will use "mainType" below for our checks, and only use ext for deciding what to do when there is no importer type. (ie: markdown files, probably)
	        var ext = Path.GetExtension(node.file).ToLower();
            var filePathAsProjectPath = FromRelativeToProjectPath(ctx.assetPath, node.file);
    
            //check the file type we are referencing.
            var mainType = AssetDatabase.GetMainAssetTypeAtPath(filePathAsProjectPath);

            //Node that references another canvas
            if (mainType == typeof(CanvasObject))
            {
	            var cn = new CanvasNode(filePathAsProjectPath,n);
				cn.Asset = AssetDatabase.LoadAssetAtPath<CanvasObject>(filePathAsProjectPath);
	            return cn;
            }else if(mainType == typeof(Texture2D))//references an image
            {
	            //Unity handles this for us:
	            //jpg, jpeg, tif, tiff, tga, gif, png, psd, bmp, iff, pict, pic, pct, exr, hdr
	            //So we don't need to check file types. Nice! If we did, then custom PSD importer would break if it is/isn't installed, and such weird edge cases.
	            var texture2DNode = new Texture2DNode(filePathAsProjectPath,n);
	            texture2DNode.Asset = AssetDatabase.LoadAssetAtPath<Texture2D>(filePathAsProjectPath);
	            return texture2DNode;
            }else if (mainType == typeof(Sprite))//references an image, but we're in a 2D project or importer settings are set to sprite...
            {
	            var spriteNode = new SpriteNode(filePathAsProjectPath,n);
	            spriteNode.Asset = AssetDatabase.LoadAssetAtPath<Sprite>(filePathAsProjectPath);
	            return spriteNode;
            }else if (mainType == typeof(TextAsset))
            {
	            var an = new TextAssetNode(filePathAsProjectPath,n);
	            an.Asset = AssetDatabase.LoadAssetAtPath<TextAsset>(an.FilePath);
	            return an;
            }
            else
            {
	            //mainType is not handled.... but that doesn't mean we've failed!
	            //In fact, its PROBABLY that we haven't, because unity doesnt' handle ".md" files.
	            //You could use a third-party markdown text importer for Unity
	            if (textAssetExtensions.Contains(ext.ToLower()))
	            {
		            var an = new TextAssetNode(filePathAsProjectPath, n);
		            an.Asset = AssetDatabase.LoadAssetAtPath<TextAsset>(an.FilePath);
		            return an;
	            }else if (ext == ".canvas")
	            {
		            //if it's a canvas file, which we already should have handled, but this one hasn't been imported yet because we are importing multiple at the same time that reference each other. What order do they import in?..... wait how do we like, fix this bug properly? uhh
		            var cn = new CanvasNode(filePathAsProjectPath, n);
		            cn.Asset = AssetDatabase.LoadAssetAtPath<CanvasObject>(filePathAsProjectPath);
		            return cn;
	            }
	            //else...
	            Debug.LogWarning($"Node Asset type (t:{mainType},ext:{ext}) is not handled. If this looks wrong, then I'm sorry. Ignoring node.");
	            return null;
            }
            
            // Sadly this isn't how c# works lol
	        // var assetNode = new AssetNode<mainType>(n);
	        
    	}
    	else if (node.type == "link")
    	{
    		var ln = new LinkNode(n);
    		ln.url = node.url;
    		return ln;
    	}
        else if (node.type == "group")
        {
	        //"label" may be null, but if so, the JSON deserializer will just use default value, empty string.
	        var gn = new GroupNode(node.label,n);
	        if (node.background != null)
	        {
		        gn.Background = AssetDatabase.LoadAssetAtPath<Texture2D>(FromRelativeToProjectPath(ctx.assetPath, node.background));
		        gn.BackgroundStyle = GetBackgroundStyle(node.backgroundStyle);
	        }

	        return gn;
        }
        
    	return n;
    }

	//I know there's better ways to do string-> enum than these functions
	//and that newtonsoft JSON can do [JsonConverter(typeof(StringEnumConverter))]
	//but this is FINE. uhg.
	public static EdgeEnd GetEdgeEnd(string end, EdgeEnd fallback)
	{
		if (string.IsNullOrEmpty(end))
		{
			return fallback;
		}
		switch (end)
		{
			case "none":
				return EdgeEnd.None;
			case "arrow":
				return EdgeEnd.Arrow;
		}

		return EdgeEnd.None;
	}
	public static NodeSide GetNodePortSide(string side)
	{
		switch (side)
		{
			case "top":
				return NodeSide.Top;
			case "right":
				return NodeSide.Right;
			case "bottom":
				return NodeSide.Bottom;
			case "left":
				return NodeSide.Left;
		}

		return NodeSide.Left;
	}
	public static BackgroundStyle GetBackgroundStyle(string nodeBackgroundStyle)
	{
		switch (nodeBackgroundStyle)
		{
			case "cover":
				return BackgroundStyle.Cover;
			case "ratio":
				return BackgroundStyle.Ratio;
			case "repeat":
				return BackgroundStyle.Repeat;
		}

		return BackgroundStyle.Cover;
	}

	/// <summary>
	/// Returns a path relative to project (/assets/), given some path relative to this canvas asset. Does not check for validity, just concats some strings.
	/// </summary>
	private string FromRelativeToProjectPath(string assetPath,string relPath)
	{
		//path is relative to the canvas, which is this.
		var dir = Path.GetDirectoryName(assetPath) + "\\";
		return dir + relPath;
	}

}

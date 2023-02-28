using System;
using System.IO;
using ObsidianCanvas.MarkdownData;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, new[]{"markdown"}, new []{"md"})]
public class MarkdownImporter : ScriptedImporter
{
	public override void OnImportAsset(AssetImportContext ctx)
	{
		var markdownObject = ScriptableObject.CreateInstance<MarkdownObject>();
		markdownObject.name = Path.GetFileNameWithoutExtension(ctx.assetPath) + " obj";

		markdownObject.SetText(File.ReadAllText(ctx.assetPath));
		
		EditorUtility.SetDirty(markdownObject);
		ctx.AddObjectToAsset("Canvas Object", markdownObject);
		ctx.SetMainObject(markdownObject);
	}


}

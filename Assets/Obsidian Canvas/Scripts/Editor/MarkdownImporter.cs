using System;
using System.IO;
using System.Linq;
using ObsidianCanvas.Frontmatter;
using ObsidianCanvas.MarkdownData;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using YAMLUtility;

[ScriptedImporter(1, new[]{"markdown"}, new []{"md"})]
public class MarkdownImporter : ScriptedImporter
{
	private const string frontmatterDelim = "---";
	//todo: this needs to not be empty by default
	//but we don't know if there are any classes?
	//so throw errors correctly and skip frontmatter until assigned
	[SerializeField]
	private string selectedTypeName;
	public override void OnImportAsset(AssetImportContext ctx)
	{
		var markdownObject = ScriptableObject.CreateInstance<MarkdownObject>();
		var assetName = Path.GetFileNameWithoutExtension(ctx.assetPath);
		markdownObject.name = assetName + " obj";
		
		//parse the text file.

		var data = SeparateFrontmatter(File.ReadAllText(ctx.assetPath));
		//Create the frontmatter object
		//Type markdownImportType = Type.GetType(selectedTypeName);//doesnt work?
		var markdownImportType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(p => typeof(IFrontmatter).IsAssignableFrom(p) && p.Name == selectedTypeName);

		//create textAsset for just the body text.

		var bodyAsset = new TextAsset(data.Item2);
		markdownObject.Init(markdownImportType,data.Item1,bodyAsset);
		ctx.AddObjectToAsset(assetName+" body",bodyAsset);
		bodyAsset.name = assetName + " body";
		EditorUtility.SetDirty(markdownObject);
		ctx.AddObjectToAsset("Canvas Object", markdownObject);
		ctx.SetMainObject(markdownObject);
	}

	public static  (string, string) SeparateFrontmatter(string markdown)
	{
		if (string.IsNullOrEmpty(markdown) )
		{
			return ("", "");
		}

		string[] lines = markdown.Split('\n');
		if (lines.Length <3)
		{
			//too short to have frontmatter. you need two lines of frontmatter deliminaters + content
			return ("", markdown);
		}

		//No frontmatter. First line MUST be '---' for obsidian.
		if (lines[0] != frontmatterDelim)
		{
			return ("", markdown);
		}

		int separation = 0;
		//now we have to find the second deliminator.
		for (int i = 1; i < lines.Length; i++)
		{
			if (lines[i] == frontmatterDelim)
			{
				separation = i;
				break;
			}
		}

		if (separation == 0)
		{
			return ("", markdown);
		}
		
		//now lines between 0 and separation is frontmatter
		//and lines after sepearation is content.
		string frontmatter = string.Join("\n", lines,1, separation - 1);
		string justMarkdown = string.Join("\n",lines, separation + 1, lines.Length - separation-1);
		return (frontmatter, justMarkdown);
	}
}

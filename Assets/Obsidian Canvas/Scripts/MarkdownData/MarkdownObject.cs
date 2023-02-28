using System;
using UnityEditor;
using UnityEngine;

namespace ObsidianCanvas.MarkdownData
{
	public class MarkdownObject : ScriptableObject
	{
		//Currently there is no advantage to using this importer.
		//but... frontmatter support into objects!
		
		[TextArea(10,30)]
		public string text;

		public void ParseFrontmatter()
		{
			//step 1: find frontmatter
				//split by end-line, search for "---" or ... lines that have at least, but any number of, dashes?
			//step 2: parse as YAML using https://github.com/aaubry/YamlDotNet
			//MarkdownObject<T> where T is what we try to parse front-matter into, the rest is markdown.
			
			//Step 3: markdown to textMeshPro-compatible rtf using some converter.
		}

		public void SetText(string text)
		{
			this.text = text;
		}
	}

	[Serializable]
	public class FrontmatterProperty
	{
		private string id;
		private object value;
	}
}
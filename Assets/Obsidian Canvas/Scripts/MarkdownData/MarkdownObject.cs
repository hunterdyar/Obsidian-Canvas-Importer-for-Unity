using System;
using System.Reflection;
using ObsidianCanvas.Frontmatter;
using UnityEditor;
using UnityEngine;
using YAMLUtility;

namespace ObsidianCanvas.MarkdownData
{
	public class MarkdownObject : ScriptableObject
	{
		//Currently there is no advantage to using this importer.
		//but... frontmatter support into objects!

		[SerializeReference]
		public object frontmatter;
		
		[TextArea(10,20)]
		public string frontmatterText;

		public TextAsset body;
		public void ParseFrontmatter(Type frontType, string front)
		{
			frontmatterText = front;
			//lol this is so gross. we're just throwing type safety out the window.
			//but like... yeah. Kind of the point?
			MethodInfo method = typeof(SimpleFrontmatterParse).GetMethod("FromYAML");
			method = method.MakeGenericMethod(new Type[] { frontType });

			frontmatter = method?.Invoke(this, new object[] { front });
		}

		public void Init(Type frontType, string front, TextAsset body)
		{
			ParseFrontmatter(frontType,front);
			this.body = body;
		}

		public T GetFrontmatter<T>() where T : IFrontmatter
		{
			return (T)frontmatter;
		}
	}
}
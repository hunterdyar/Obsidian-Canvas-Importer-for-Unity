using System;
using System.Collections.Generic;
using System.Linq;
using ObsidianCanvas.MarkdownData;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Obsidian_Canvas.Scripts.Editor
{
	[CustomEditor(typeof(MarkdownImporter))]
	public class MarkdownImporterEditor : ScriptedImporterEditor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.LabelField("Frontmatter Type");
			var ifrontmatterType = typeof(IFrontmatter);
			var typeProp = serializedObject.FindProperty("selectedTypeName");
			var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(p => ifrontmatterType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

			//todo: how to convert with linq?
			List<string> names = new List<string>();
			foreach (var t in types)
			{
				names.Add(t.Name);
			}

			string[] typeNames = names.ToArray();
			int selected = names.IndexOf(typeProp.stringValue);//indexOf
			int newSelected = EditorGUILayout.Popup(selected,typeNames);
			if (selected != newSelected)
			{
				typeProp.stringValue = types.ToList()[newSelected].Name;
				serializedObject.ApplyModifiedProperties();
			}
			base.ApplyRevertGUI();
			
		}
	}
}
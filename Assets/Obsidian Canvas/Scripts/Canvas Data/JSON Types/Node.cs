using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObsidianCanvas.JSONTypes
{
	[Serializable]
	public class Node
	{
		public string id;
		public int x;
		public int y;
		public int width;
		public int height;
		
		//the different types of obsidian nodes only have a few properties. Json deserializer will try or fail to import them.
		//We get them all here in the master "Node" json deserialized object and then create proper Node Objects after.
		public string type;
		public string file;//path for file data for type 'file'.
		public string text;//text for text nodes for type 'text'.
		public string url;//url for links to external resources for, basically, also files; but not a markdown file.
		public string label;
		public string background;//background image for group nodes.

		// [JsonConverter(typeof(StringEnumConverter))]
		public string backgroundStyle;
		
		public Color? color;
	}
}
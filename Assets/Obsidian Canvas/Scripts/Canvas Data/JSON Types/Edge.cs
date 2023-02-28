using System;
using UnityEngine;

namespace ObsidianCanvas.JSONTypes
{
	[Serializable]
	public class Edge
	{
		//Json Deserialize
		
		public string id;
		public string fromNode;//the string id
		public string fromSide;
		public string fromEnd;
		
		public string toNode;//the string id
		public string toSide;
		public string toEnd;

		public Color? color;

		public string label = "";
	}
}
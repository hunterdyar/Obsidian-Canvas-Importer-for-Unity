using System;
using UnityEngine;

namespace ObsidianCanvas.Data
{
	[Serializable]
	public class Port
	{
		//set outside of constructor for convenience.
		//we can get away with these odd extra messy fluff, because this is all from file data and really shouldn't be edited at runtime
		//if that wasn't true, this would be gross.
		public Port OtherPort;
		
		//set in constructor
		[SerializeReference]
		public Node MyNode;
		[SerializeReference]
		public Node ConnectedNode;//the OTHER node.
		public EdgeEnd PortEnd;//for the owner node
		public NodeSide PortSide;//for the owner node
		public string Label;
		public Color? Color;
		public Port(Node myNode,Node connectedNode, NodeSide side, EdgeEnd end, Color? color, string label)
		{
			MyNode = myNode;
			ConnectedNode = connectedNode;
			PortEnd = end;
			PortSide = side;
			Color = color;
			Label = label;
		}

		/// <summary>
		/// Returns an edge color, but will return a value. The Color value you can access directly is nullable, so may not.
		/// </summary>
		/// <param name="fallbackColor">(optional) Color to return if color is null. If not set, will be default</param>
		public Color GetColor(Color fallbackColor = default)
		{
			if (Color == null)
			{
				return fallbackColor;
			}

			return (Color)Color;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ObsidianCanvas.Data
{
	[Serializable]
	public class Node
	{
		public string ID;
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public Color Color;
		
		[SerializeReference]
		public List<GroupNode> Groups = new List<GroupNode>();

		public bool IsGroupNode => this is GroupNode;
		public Rect Rect => _rect;
		private Rect _rect;
		[SerializeReference]
		public List<Port> Connections = new List<Port>();
		public Node(Node node)
		{
			this.ID = node.ID;
			this.X = node.X;
			this.Y = node.Y;
			this.Width = node.Width;
			this.Height = node.Height;
			this.Color = node.Color;

			_rect = new Rect(X, Y, Width, Height);
		}


		public Node(JSONTypes.Node JSONNode,Color defaultColor)
		{
			
			ID = JSONNode.id;
			Width = JSONNode.width;
			Height = JSONNode.height;
			X = JSONNode.x;
			Y = JSONNode.y;
			if (JSONNode.color != null)
			{
				this.Color = (Color)JSONNode.color;
			}
			else
			{
				this.Color = defaultColor;
			}
		}

		public void AddPort(Port port)
		{
			//todo: check if we are not already connected to this node, is valid, etc.
			if (port != null)
			{
				Connections.Add(port);
			}
			else
			{
				Debug.LogWarning("Import Edge for canvas failed with null port?");
			}
		}

		public List<Node> ConnectedNodes()
		{
			return Connections.ConvertAll(x => x.ConnectedNode);
		}

		/// <summary>
		/// Returns a node color. Will return a value. The Color value you can access directly is nullable, so may not.
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
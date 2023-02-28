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
			_rect = new Rect(X, Y, Width, Height);
		}


		public Node(JSONTypes.Node JSONNode)
		{
			ID = JSONNode.id;
			Width = JSONNode.width;
			Height = JSONNode.height;
			X = JSONNode.x;
			Y = JSONNode.y;
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
		
	}
}
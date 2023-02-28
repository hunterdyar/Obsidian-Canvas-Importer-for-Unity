using System;
using System.Collections.Generic;
using ObsidianCanvas.Data;
using UnityEngine;
using UnityEngine.Serialization;
using Node = ObsidianCanvas.Data.Node;

namespace ObsidianCanvas
{
	//I don't use [CreateAssetMenu] because I wrote the code assuming it's all deterministic from a file and won't be changed from runtime unless someone knows what theyre doing.
	//For example, Connections have multiple sources of truth.
	public class CanvasObject : ScriptableObject
	{
		[FormerlySerializedAs("_nodes")] [SerializeReference]
		public List<Node> nodes = new();

		//used to convert the relative File paths from obsidian to project paths
		//we can't ask the assetDatabase for our path yet... because we don't have one yet! This is during import.
		private string _path;
		
		public Node GetNode(string id)
		{
			return nodes.Find(x => x.ID == id);
		}

		public void SetPath(string ctxAssetPath)
		{
			_path = ctxAssetPath;
		}

		public void Clear()
		{
			nodes.Clear();
			// _edges.Clear();
		}

		public void AddNode(Node node)
		{
			if (node != null)
			{
				nodes.Add(node);
			}
		}
		

		// public void AddEdge(Edge edge)
		// {
		// 	_edges.Add(edge.id,edge);
		// }

		public void ConnectNodes(string edgeFromNode, NodeSide edgeFromSide, EdgeEnd edgeFromEnd, NodeSide edgeToSide, string edgeToNode, EdgeEnd edgeToEnd, Color? color, string label)
		{
			Node fromNode = GetNode(edgeFromNode);
			Node toNode = GetNode(edgeToNode);
			if (fromNode != null && toNode != null)
			{
				//in obsidian, you can't have a one-sided node. so "port" isn't the right word...
				//"edge" is, but we are not keeping a separate list of edges.
				Port a = new Port(fromNode,toNode, edgeFromSide, edgeFromEnd,color,label);
				Port b = new Port(toNode,fromNode, edgeToSide, edgeToEnd,color, label);
				
				a.OtherPort = b;
				b.OtherPort = a;
				
				fromNode.AddPort(a);
				toNode.AddPort(b);
			}
		}

		/// <summary>
		/// A starting node is any node with at least one connection, and all connections pointed away from the node.
		/// </summary>
		/// <returns></returns>
		public List<Node> GetAllStartingNodes()
		{
			Debug.Log($"Searching for starting nodes. There are {nodes.Count} total nodes.");
			var starting = new List<Node>();
			foreach (var node in nodes)
			{
				if (node.Connections.Count > 0)
				{
					bool isStarting = true;
					foreach (var port in node.Connections)
					{
						if (port.PortEnd == EdgeEnd.Arrow)
						{
							isStarting = false;
							break;
						}
					}

					if (isStarting)
					{
						starting.Add(node);
					}
				}
			}

			return starting;
		}
	}
}
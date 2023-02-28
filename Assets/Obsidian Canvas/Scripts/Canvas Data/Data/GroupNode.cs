using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class GroupNode : Node
	{
		public string Label;
		public Texture2D Background;
		public BackgroundStyle BackgroundStyle = BackgroundStyle.Cover;
		[SerializeReference] public List<Node> ChildrenNodes = new List<Node>();
		public GroupNode(string label, Node node) : base(node)
		{
			Label = label;
		}

		/// <summary>
		/// Returns Children of this group, excluding children nodes that are GroupNodes.
		/// </summary>
		public List<Node> GetChildrenExcludingGroupNodes()
		{
			return ChildrenNodes.Where(n => !n.IsGroupNode).ToList();
		}
	}
}
using UnityEngine;

namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class GroupNode : Node
	{
		public string Label;
		public Texture2D Background;
		public BackgroundStyle BackgroundStyle = BackgroundStyle.Cover;

		public GroupNode(string label, Node node) : base(node)
		{
			Label = label;
		}
	}
}
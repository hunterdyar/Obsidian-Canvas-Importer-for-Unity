using System;

namespace ObsidianCanvas.Data
{
	[Serializable]
	public class LinkNode : Node
	{
		public string url;
		public LinkNode(Node node) : base(node)
		{
		}
	}
}
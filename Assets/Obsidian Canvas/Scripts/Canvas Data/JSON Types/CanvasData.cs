using System;

namespace ObsidianCanvas.JSONTypes
{
	[Serializable]
	public class CanvasData
	{
		public Node[] nodes;
		public Edge[] edges;
	}
}
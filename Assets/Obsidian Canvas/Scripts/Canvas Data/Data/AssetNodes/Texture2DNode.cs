using UnityEngine;

namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class Texture2DNode : AssetNode<Texture2D>
	{
		public Texture2D Image => Asset;
		public Texture2DNode(string filePath, Node node) : base(filePath,node)
		{
		}
	}
}
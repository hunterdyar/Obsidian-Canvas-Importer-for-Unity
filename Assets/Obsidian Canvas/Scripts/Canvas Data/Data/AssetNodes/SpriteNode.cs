using UnityEngine;

namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class SpriteNode : AssetNode<Sprite>
	{
		public Sprite Sprite => Asset;
		public SpriteNode(string filePath, Node node) : base(filePath,node)
		{
		}
	}
}
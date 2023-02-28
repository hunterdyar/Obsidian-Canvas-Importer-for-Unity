using UnityEngine;

namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class TextAssetNode : AssetNode<TextAsset>
	{
		public TextAssetNode(string filePath, Node node) : base(filePath,node)
		{
		}
	}
}
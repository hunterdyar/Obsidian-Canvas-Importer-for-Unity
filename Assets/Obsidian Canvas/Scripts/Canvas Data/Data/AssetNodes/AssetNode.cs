using UnityEngine;
using UnityEngine.Windows;

namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class AssetNode<T> : Node
	{
		public string FilePath;
		[SerializeReference]
		public T Asset;

		public AssetNode(string filePath,Node node) : base(node)
		{
			FilePath = filePath;
		}
	}
}
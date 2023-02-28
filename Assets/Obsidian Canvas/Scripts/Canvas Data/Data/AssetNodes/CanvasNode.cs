namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class CanvasNode : AssetNode<CanvasObject>
	{
		//renaming just for convenience
		public CanvasObject Canvas => Asset;

		public CanvasNode(string filePath, Node node) : base(filePath,node)
		{
		}
	}
}
namespace ObsidianCanvas.Data
{
	[System.Serializable]
	public class TextNode : Node
	{
		public string Text;
		public TextNode(string text, Node node) : base(node)
		{
			Text = text;
		}
	}
}
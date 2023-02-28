using System;
using ObsidianCanvas.MarkdownData;

namespace ObsidianCanvas.Frontmatter
{
	[Serializable]
	public class BasicFrontmatterType : IFrontmatter
	{
		public string title;
		public int size;
		public float amount;
		public bool active;

		public BasicFrontmatterType()
		{
			
		}
	}
}
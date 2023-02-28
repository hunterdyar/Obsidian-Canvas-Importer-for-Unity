using UnityEngine;

namespace ObsidianCanvas
{
	public static class Extensions
	{
		public static bool Contains(this Rect a, Rect other)
		{
			return a.Contains(other.min) && a.Contains(other.max);
		}
	}
}
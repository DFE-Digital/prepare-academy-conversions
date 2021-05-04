using System.Collections.Generic;

namespace ApplyToBecomeInternal.Extensions
{
	public static class StringExtensions
	{
		public static IEnumerable<int> AllIndicesOf(this string str, string value)
		{
			for (var index = 0;; index += value.Length)
			{
				index = str.IndexOf(value, index);
				if (index == -1)
					break;
				yield return index;
			}
		}
	}
}
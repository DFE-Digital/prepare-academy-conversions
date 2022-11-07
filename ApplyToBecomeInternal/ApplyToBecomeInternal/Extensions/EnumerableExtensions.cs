using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();

			return from TSource element in source
				   where seenKeys.Add(keySelector(element))
				   select element;
		}
	}
}

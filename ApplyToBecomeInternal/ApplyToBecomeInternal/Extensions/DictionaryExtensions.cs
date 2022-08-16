using System.Collections.Generic;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary.ContainsKey(key)) return dictionary[key];

			return default(TValue);
		}
	}
}

using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Models
{
	public class StringTemplate
	{
		private readonly string _template;
		private readonly Dictionary<string, string> _data = new Dictionary<string, string>();
		
		public StringTemplate(string template) => _template = template;
		
		private static string GetFullKey(string key) => $"{{{key}}}";

		public void Set(string key, string value) => _data[key] = value;

		public override string ToString()
		{
			var indices = new Dictionary<int, string>();
			foreach ((string key, string _) in _data)
			{
				string fullKey = GetFullKey(key);
				for (int index = 0;; index += fullKey.Length)
				{
					index = _template.IndexOf(fullKey, index);
					if (index == -1)
						break;
					indices[index] = key;
				}
			}

			var sortedIndices = indices.OrderBy(pair => pair.Key);

			var indexAdjustment = 0;
			var result = _template;
			foreach ((int index, string key) in sortedIndices)
			{
				var fullKey = GetFullKey(key);
				result = result.Remove(index + indexAdjustment, fullKey.Length);
				result = result.Insert(index + indexAdjustment, _data[key]);
				indexAdjustment += _data[key].Length - fullKey.Length;
			}

			return result;
		}
	}
}
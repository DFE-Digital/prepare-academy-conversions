using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Models
{
	public class StringTemplate
	{
		private readonly string _template;
		private readonly Dictionary<string, string> _data = new Dictionary<string, string>();
		
		public StringTemplate(string template) => _template = template;
		
		private static string GetFullKey(string key) => $"{{{key}}}"; // {key}

		public void Set(string key, string value) => _data[key] = value;

		public override string ToString()
		{
			var indices = new Dictionary<int, string>();
			foreach (var (key, _) in _data)
			{
				var fullKey = GetFullKey(key);
				foreach (var i in _template.AllIndicesOf(fullKey)) 
					indices[i] = key;
			}

			var sortedIndices = indices.OrderBy(pair => pair.Key);

			var indexAdjustment = 0;
			var result = _template;
			foreach (var (index, key) in sortedIndices)
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
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Models
{
	public class StringTemplate
	{
		private readonly string _template;
		private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

		public StringTemplate(string template) => _template = template;

		public void Set(string key, string value) => _data[key] = value;

		public override string ToString() => _data.Aggregate(_template, (current, datum) => current.Replace($"{{{datum.Key}}}", datum.Value));
	}
}
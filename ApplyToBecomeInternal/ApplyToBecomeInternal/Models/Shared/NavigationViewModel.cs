using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Models.Shared
{
	public class NavigationViewModel
	{
		public NavigationViewModel(NavigationTarget target):this(target, Enumerable.Empty<KeyValuePair<string, string>>())
		{
		}

		public NavigationViewModel(NavigationTarget target, IEnumerable<KeyValuePair<string, string>> urlTemplateData)
		{
			var (content, url) = target.GetContent();
			Content = content;
			var template = new StringTemplate(url);
			foreach ((string key, string value) in urlTemplateData)
				template.Set(key, value);
			Url = template.ToString();
		}

		public string Content { get; }
		public string Url { get; }
	}
}

using ApplyToBecomeInternal.Extensions;
using System;

namespace ApplyToBecomeInternal.Models.Shared
{
	public class NavigationViewModel
	{
		public NavigationViewModel(NavigationTarget target, string id)
		{
			var (content, url) = target.GetContent();
			Content = content;
			Url = BuildUrlWithId(url, id);
		}

		public NavigationViewModel(NavigationTarget target)
		{
			var (content, url) = target.GetContent();
			Content = content;
			Url = url;
		}

		public string Content { get; }
		public string Url { get; }
		private string BuildUrlWithId(string url, string id)
		{
			return url.Replace("{id}", id);
		}
	}
}

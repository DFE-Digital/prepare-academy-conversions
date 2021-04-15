using ApplyToBecomeInternal.Extensions;

namespace ApplyToBecomeInternal.Models.Shared
{
	public class NavigationViewModel
	{
		public NavigationViewModel(NavigationTarget target)
		{
			var (content, url) = target.GetContent();
			Content = content;
			Url = url;
		}

		public string Content { get; }
		public string Url { get; }
	}
}

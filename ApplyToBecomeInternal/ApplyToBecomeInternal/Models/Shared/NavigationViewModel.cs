using ApplyToBecomeInternal.Extensions;

namespace ApplyToBecomeInternal.Models.Shared
{
	public class NavigationViewModel
	{
		public NavigationViewModel(NavigationContent content)
		{
			Content = NavigationExtensions.GetAttribute<NavigationAttribute>(content).Content;
			Url = NavigationExtensions.GetAttribute<NavigationAttribute>(content).Url;
		}

		public string Content { get; }
		public string Url { get; set; }
	}
}

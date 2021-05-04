using System;

namespace ApplyToBecomeInternal.Models.Navigation
{
	public class NavigationAttribute : Attribute
	{
		public NavigationAttribute(string content, string url)
		{
			Content = content;
			Url = url;
		}
		
		public string Content { get; }
		public string Url { get; }
	}
}

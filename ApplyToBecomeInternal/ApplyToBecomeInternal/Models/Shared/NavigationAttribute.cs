using System;

namespace ApplyToBecomeInternal.Models.Shared
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

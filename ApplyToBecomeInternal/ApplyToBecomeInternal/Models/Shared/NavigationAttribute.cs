using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models.Shared
{
	public class NavigationAttribute : Attribute
	{
		public NavigationAttribute(string content, string url)
		{
			Content = content;
			Url = url;
		}
		public string Content { get; private set; }
		public string Url { get; private set; }
	}
}

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision
{
	public class BackLink
	{
		public BackLink(string linkPage, Dictionary<string, string> linkRouteValues, string linkText = "Back")
		{
			LinkPage = linkPage;
			LinkRouteValues = linkRouteValues;
			LinkText = linkText;
		}

		public string LinkPage { get; set; }
		public Dictionary<string, string> LinkRouteValues { get; set; }
		public string LinkText { get; set; }

		//public string ToQueryString(string path)
		//{
		//	var sb = new StringBuilder(path);

		//	for (int i = 0; i < LinkRouteValues.Count; i++)
		//	{
		//		var routeValue = LinkRouteValues.ElementAt(i);
		//		if (i == 0) sb.Append("?");
		//		else
		//		{
		//			sb.Append("&");
		//		}

		//		sb.Append($"{routeValue.Key}={routeValue.Value}");
		//	}
		//	return sb.ToString();
		//}
	}
}

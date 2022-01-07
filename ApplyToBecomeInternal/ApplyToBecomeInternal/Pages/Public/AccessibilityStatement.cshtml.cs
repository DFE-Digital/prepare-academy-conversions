using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.Public
{
	public class AccessibilityStatement : PageModel
	{
		public void OnGet(string returnUrl)
		{
			ReturnLink = new LinkItem { BackText = "Back", Page = returnUrl };
		}

		public LinkItem ReturnLink { get; set; }
	}
}
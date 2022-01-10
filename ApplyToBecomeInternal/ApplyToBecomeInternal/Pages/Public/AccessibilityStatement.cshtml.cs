using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.Public
{
	public class AccessibilityStatement : PageModel
	{		
		public void OnGet(string returnUrl)
		{
			ReturnLink = returnUrl;
		}

		public string ReturnLink { get; set; }
	}
}
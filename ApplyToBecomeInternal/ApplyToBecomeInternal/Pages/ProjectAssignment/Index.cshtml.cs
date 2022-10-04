using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ProjectAssignment
{
    public class IndexModel : PageModel
    {
		public string[] DeliveryOfficers { get; set; }
        public void OnGet()
        {
	        DeliveryOfficers = new[] { "John Smith", "Jane Doe" };
        }
    }
}

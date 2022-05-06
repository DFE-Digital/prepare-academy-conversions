using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.Errors
{
    public class IndexModel : PageModel
    {
	    public string ErrorMessage { get; private set; } = "An error occurred while processing your request";

		public void OnGet(int? statusCode = null)
		{
			if (!statusCode.HasValue)
			{
				return;
			}

			ErrorMessage = statusCode.Value switch
			{
				404 => "Page not found",
				500 => "Internal server error",
				501 => "Not implemented",
				_ => $"Error {statusCode}"
			};
		}
    }
}

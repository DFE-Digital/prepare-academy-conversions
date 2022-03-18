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
		public string ErrorMessage { get; set; } 

		public void OnGet(int? statusCode = null)
        {
			if (statusCode.HasValue)
			{
				switch (statusCode.Value)
				{
					case 404: ErrorMessage = "Page not found";
						break;
					case 500: ErrorMessage = "Internal server error";
						break;
					case 501: ErrorMessage = "Not implemented";
						break;
					default: ErrorMessage = $"Error {statusCode}";
						break;
				}
			}
        }
    }
}

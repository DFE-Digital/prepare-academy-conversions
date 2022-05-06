using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
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
				ManageUnhandledErrors();
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

		public void OnPost(int? statusCode = null)
		{
			if (!statusCode.HasValue)
			{
				ManageUnhandledErrors();
			}
		}

		private void ManageUnhandledErrors()
		{
			var unhandledError = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error;

			if (unhandledError is InvalidOperationException && unhandledError.Message.ToLower().Contains("no page named"))
			{
				ErrorMessage = "Page not found";
				HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
			}
		}
	}
}
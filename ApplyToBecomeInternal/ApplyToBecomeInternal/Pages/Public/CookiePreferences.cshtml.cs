using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ApplyToBecomeInternal.Pages.Public
{
	public class CookiePreferences : PageModel
	{
		private const string ConsentCookieName = ".ManageAnAcademyConversion.Consent";
		public bool? Consent { get; set; }

		public ActionResult OnGet(bool? consent, string returnUrl)
		{
			if (Request.Cookies.ContainsKey(ConsentCookieName))
			{
				Consent = bool.Parse(Request.Cookies[ConsentCookieName]);
			}

			if (consent.HasValue)
			{
				var cookieOptions = new CookieOptions { Expires = DateTime.Today.AddMonths(6), Secure = true };
				Response.Cookies.Append(ConsentCookieName, consent.Value.ToString(), cookieOptions);

				if (!consent.Value)
				{
					foreach (var cookie in Request.Cookies.Keys)
					{
						if (cookie.StartsWith("_ga") || cookie.Equals("_gid"))
						{
							Response.Cookies.Delete(cookie);
						}
					}
				}

				if (!string.IsNullOrEmpty(returnUrl))
				{
					return Redirect(returnUrl);
				}

				return RedirectToPage(Links.Public.CookiePreferences);
			}

			return Page();
		}
	}
}
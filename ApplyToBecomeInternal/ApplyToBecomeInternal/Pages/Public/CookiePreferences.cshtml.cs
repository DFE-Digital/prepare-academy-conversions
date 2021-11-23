using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net;

namespace ApplyToBecomeInternal.Pages.Public
{
	public class CookiePreferences : PageModel
	{
		[BindProperty(Name = "consent")] public bool Consent { get; set; }
		[BindProperty(Name = "return")] public string Return { get; set; }

		public void OnGet()
		{
		}

		public ActionResult OnPost()
		{
			var cookieOptions = new CookieOptions { Expires = DateTime.Today.AddMonths(6), Secure = true };
			Response.Cookies.Append(".ManageAnAcademyConversion.Consent", Consent.ToString(), cookieOptions);

			if (!string.IsNullOrEmpty(Return))
			{
				return RedirectToPage(Return);
			}

			return RedirectToPage(Links.Public.CookiePreferences);
		}
	}
}
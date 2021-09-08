using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.Login
{
	public class Index : PageModel
	{
		private readonly IConfiguration _configuration;
		public string ReturnUrl { get; set; }

		public Index(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		
		public void OnGet(string returnUrl)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string username, string password, string returnUrl)
		{
			var decodedUrl = "";

			if (username != _configuration["Authentication:Username"] || password != _configuration["Authentication:Password"])
			{
				TempData["Error.Message"] = "Incorrect username and password";
				return Page();
			}

			var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Name") };

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authenticationProperties = new AuthenticationProperties();
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity), authenticationProperties);

			if (!string.IsNullOrEmpty(returnUrl))
			{
				decodedUrl = WebUtility.UrlDecode(returnUrl);
			}

			if (Url.IsLocalUrl(decodedUrl))
			{
				return Redirect(returnUrl);
			}

			return RedirectToPage(Links.ProjectList.Index.Page);
		}

		public async Task<IActionResult> OnGetSignOut()
		{
			await HttpContext.SignOutAsync();
			return RedirectToPage(Links.Login.LoginForm.Page);
		}
	}
}
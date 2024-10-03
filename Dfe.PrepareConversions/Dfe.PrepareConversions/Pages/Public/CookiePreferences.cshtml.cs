using Dfe.PrepareConversions.Configuration;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Dfe.PrepareConversions.Pages.Public;

public class CookiePreferences(ILogger<CookiePreferences> logger, IOptions<ServiceLinkOptions> options) : PageModel
{
   private readonly string[] CONSENT_COOKIE_NAMES = [".ManageAnAcademyConversion.Consent", ".ManageAnAcademyTransfer.Consent"];
   public bool? Consent { get; set; }
   public bool PreferencesSet { get; set; }
   public string ReturnPath { get; set; }

   public ActionResult OnGet(bool? consent, string returnUrl)
   {
      ReturnPath = returnUrl;

      if (Request.Cookies.ContainsKey(CONSENT_COOKIE_NAMES[0]) && Request.Cookies.ContainsKey(CONSENT_COOKIE_NAMES[1]))
      {
         Consent = bool.Parse(Request.Cookies[CONSENT_COOKIE_NAMES[0]] ?? string.Empty) && bool.Parse(Request.Cookies[CONSENT_COOKIE_NAMES[1]] ?? string.Empty);
      }

      if (consent.HasValue)
      {
         PreferencesSet = true; 
         ApplyCookieConsent(consent);

         if (!string.IsNullOrEmpty(returnUrl))
         {
            return Redirect(returnUrl);
         }

         return RedirectToPage(Links.Public.CookiePreferences);
      }

      return Page();
   }

   public IActionResult OnPost(bool? consent, string returnUrl)
   {
      ReturnPath = returnUrl;

      if (Request.Cookies.ContainsKey(CONSENT_COOKIE_NAMES[0]) && Request.Cookies.ContainsKey(CONSENT_COOKIE_NAMES[1]))
      {
         Consent = bool.Parse(Request.Cookies[CONSENT_COOKIE_NAMES[0]] ?? string.Empty) && bool.Parse(Request.Cookies[CONSENT_COOKIE_NAMES[1]] ?? string.Empty);
      }

      if (consent.HasValue)
      {
         Consent = consent;
         PreferencesSet = true;

         AppendCookies(consent);

         if (consent.Value is false)
         {
            ApplyCookieConsent(false);
         }

         return Page();
      }

      return Page();
   }

   private void AppendCookies(bool? consent)
   {
      foreach (var CONSENT_COOKIE_NAME in CONSENT_COOKIE_NAMES)
      {
         CookieOptions cookieOptions = new() { Expires = DateTime.Today.AddMonths(6), Secure = true, HttpOnly = true };
         Response.Cookies.Append(CONSENT_COOKIE_NAME, consent.Value.ToString(), cookieOptions);
      }
   }

   private void ApplyCookieConsent(bool? consent)
   {
      if (consent.HasValue)
      {
         AppendCookies(consent);
      }

      if (consent is false)
      {
         foreach (string cookie in Request.Cookies.Keys)
         {
            // Google Analytics
            if (cookie.StartsWith("_ga") || cookie.Equals("_gid"))
            {
               logger.LogInformation("Expiring Google analytics cookie: {cookie}", cookie);
               Response.Cookies.Append(cookie, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1), Secure = true, SameSite = SameSiteMode.Lax, HttpOnly = true });
            }
            // App Insights
            if (cookie.StartsWith("ai_"))
            {
               logger.LogInformation("Expiring App insights cookie: {cookie}", cookie);
               Response.Cookies.Append(cookie, string.Empty, new CookieOptions { Expires = DateTime.Now.AddYears(-1), Secure = true, SameSite = SameSiteMode.Lax, HttpOnly = true });
            }
         }
      }
   }
}

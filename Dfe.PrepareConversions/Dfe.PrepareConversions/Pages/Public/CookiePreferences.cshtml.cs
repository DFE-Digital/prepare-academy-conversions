using Dfe.PrepareConversions.Configuration;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Dfe.PrepareConversions.Pages.Public;

public class CookiePreferences : PageModel
{
   private const string CONSENT_COOKIE_NAME = ".ManageAnAcademyConversion.Consent";
   private readonly ILogger<CookiePreferences> _logger;
   private readonly IOptions<ServiceLinkOptions> _options;

   public CookiePreferences(ILogger<CookiePreferences> logger, IOptions<ServiceLinkOptions> options)
   {
      _logger = logger;
      _options = options;
   }

   public bool? Consent { get; set; }
   public bool PreferencesSet { get; set; }
   public string ReturnPath { get; set; }

   public string TransfersCookiesUrl { get; set; }

   public ActionResult OnGet(bool? consent, string returnUrl)
   {
      ReturnPath = returnUrl;
      TransfersCookiesUrl = $"{_options.Value.TransfersUrl}/cookie-preferences?returnUrl=%2Fhome";

      if (Request.Cookies.ContainsKey(CONSENT_COOKIE_NAME))
      {
         Consent = bool.Parse(Request.Cookies[CONSENT_COOKIE_NAME] ?? string.Empty);
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

      if (Request.Cookies.ContainsKey(CONSENT_COOKIE_NAME))
      {
         Consent = bool.Parse(Request.Cookies[CONSENT_COOKIE_NAME] ?? string.Empty);
      }

      if (consent.HasValue)
      {
         Consent = consent;
         PreferencesSet = true;

         CookieOptions cookieOptions = new() { Expires = DateTime.Today.AddMonths(6), Secure = true, HttpOnly = true };
         Response.Cookies.Append(CONSENT_COOKIE_NAME, consent.Value.ToString(), cookieOptions);

         if (consent.Value is false)
         {
            ApplyCookieConsent(false);
         }

         return Page();
      }

      return Page();
   }

   private void ApplyCookieConsent(bool? consent)
   {
      if (consent.HasValue)
      {
         CookieOptions cookieOptions = new() { Expires = DateTime.Today.AddMonths(6), Secure = true, HttpOnly = true };
         Response.Cookies.Append(CONSENT_COOKIE_NAME, consent.Value.ToString(), cookieOptions);
      }

      if (consent is false)
      {
         foreach (string cookie in Request.Cookies.Keys)
         {
            if (cookie.StartsWith("_ga") || cookie.Equals("_gid"))
            {
               _logger.LogInformation("Expiring Google analytics cookie: {cookie}", cookie);
               Response.Cookies.Append(cookie, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1), Secure = true, SameSite = SameSiteMode.Lax, HttpOnly = true });
            }
         }
      }
   }
}

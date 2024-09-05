using Dfe.PrepareTransfers.Web.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Dfe.PrepareTransfers.Web.Transfers;

public static class Links
{
   private static readonly IDictionary<string, LinkItem> LinkCache = new ConcurrentDictionary<string, LinkItem>();
   private static LinkItem Create(string page, string text = "Back")
   {
      var item = new LinkItem { PageName = page, BackText = text };
      LinkCache.Add(page, item);
      return item;
   }
   public static class LegalRequirements
   {
      public static readonly LinkItem Index = Create("/Projects/LegalRequirements/Index");

      public static class IncomingTrustAgreement
      {
         public static readonly LinkItem Home =
            Create("/Projects/LegalRequirements/IncomingTrustAgreement");
      }

      public static class DiocesanConsent
      {
         public static readonly LinkItem Home = Create("/Projects/LegalRequirements/DiocesanConsent");
      }

      public static class OutgoingTrustConsent
      {
         public static readonly LinkItem Home = Create("/Projects/LegalRequirements/OutgoingTrustConsent");
      }
   }
   
   public static class HeadteacherBoard
   {
      public static readonly LinkItem Preview =
         Create("/TaskList/HtbDocument/Preview", "Back");
   }
   
   public static class Project
   {
      public static readonly LinkItem Index = Create("/Projects/Index", "Back");
   }
   
   
}
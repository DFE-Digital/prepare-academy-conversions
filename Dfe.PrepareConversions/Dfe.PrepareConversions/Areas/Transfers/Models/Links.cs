using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Dfe.PrepareTransfers.Web.Models;

public static class Links
{
    private static readonly IDictionary<string, LinkItem> LinkCache = new ConcurrentDictionary<string, LinkItem>();
    private static string _conversionsUrl;
    public static string ConversionsUrl => _conversionsUrl;

    private static LinkItem Create(string page, string text = "Back")
    {
        var item = new LinkItem { PageName = page, BackText = text };
        LinkCache.Add(page, item);
        return item;
    }
    public static void InitializeConversionsUrl(string conversionsUrl)
    {
        _conversionsUrl = conversionsUrl;
    }
    public static LinkItem FindByPageName(string page)
    {
        if (string.IsNullOrWhiteSpace(page))
        {
            return default;
        }

        return LinkCache.ContainsKey(page) ? LinkCache[page] : default;
    }

    public static class Global
    {
        public static readonly LinkItem CookiePreferences = Create(page: "/CookiePreferences");
    }

    public static class HeadteacherBoard
    {
        public static readonly LinkItem Preview =
           Create("/TaskList/HtbDocument/Preview", "Back");
    }

    public static class ProjectType
    {
        public static readonly LinkItem Index =
           Create("/ProjectType/Index", "Back");
    }

    public static class Project
    {
        public static readonly LinkItem Index = Create("/Projects/Index", "Back");
    }
   public static class DeleteProject
   {
      public static readonly LinkItem Index = Create(page: "/Projects/DeleteProject/Index");
   }

   public static class ProjectList
    {
        public static readonly LinkItem Index = Create("/Home/Index");
    }

    public static class ProjectAssignment
    {
        public static readonly LinkItem Index = Create("/Projects/ProjectAssignment/Index");
    }
    public static class TransferDateHistory
    {
        public static readonly LinkItem Index = Create("/TaskList/DateHistory/DateHistory");
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

    public static class Decision
    {
        public static readonly LinkItem RecordDecision = Create("/TaskList/Decision/RecordDecision");
        public static readonly LinkItem WhoDecided = Create("/TaskList/Decision/WhoDecided");
        public static readonly LinkItem DeclineReason = Create("/TaskList/Decision/DeclineReason");
        public static readonly LinkItem AnyConditions = Create("/TaskList/Decision/AnyConditions");
        public static readonly LinkItem DecisionDate = Create("/TaskList/Decision/DecisionDate");
        public static readonly LinkItem DecisionMaker = Create("/TaskList/Decision/DecisionMaker");
        public static readonly LinkItem WhyDeferred = Create("/TaskList/Decision/WhyDeferred");
        public static readonly LinkItem WhyWithdrawn = Create("/TaskList/Decision/WhyWithdrawn");
        public static readonly LinkItem Summary = Create("/TaskList/Decision/Summary");
        public static readonly LinkItem SubMenuRecordADecision = Create("/TaskList/Decision/RecordADecision");
        public static readonly LinkItem ApprovedInfo = Create("/TaskList/Decision/TransfersDecisionApprovedInfo");
    }

   public static class PublicSectorEqualityDutySection
   {
      public static readonly LinkItem TransferTask = Create("/Projects/PublicSectorEqualityDuty/Task");
      public static readonly LinkItem TransferLikelyhoodToImpact = Create("/Projects/PublicSectorEqualityDuty/LikelyhoodImpact");
      public static readonly LinkItem TransferImpactReductionReason = Create("/Projects/PublicSectorEqualityDuty/ImpactReductionReason");
   }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using FhaIndex = Dfe.PrepareTransfers.Web.Pages.Projects.FinancialHealthAssessment.Index;
using TransferDatesModel = Dfe.PrepareTransfers.Data.Models.Projects.TransferDates;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.Projects.FinancialHealthAssessment
{
   public class IndexTests : BaseTests
   {
      public IndexTests()
      {
         ProjectRepository.Setup(r => r.UpdateSfsoCommissioning(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);
      }

      private FhaIndex Subject() => new FhaIndex(ProjectRepository.Object) { Urn = ProjectUrn0001 };

      [Fact]
      public async Task OnGet_NoHtbDate_NotRequested()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel { HasHtbDate = false };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.False(subject.HasProposedDecisionDate);
         Assert.False(subject.RequestSent);
      }

      [Fact]
      public async Task OnGet_HtbSet_RequestNull_ShowsProposedDate()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel
         {
            Htb = "23/07/2026",
            HasHtbDate = true,
            SfsoCommissioningRequestedDate = null
         };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.True(subject.HasProposedDecisionDate);
         Assert.False(subject.RequestSent);
         Assert.Equal(new DateTime(2026, 7, 23), subject.ProposedDecisionDate);
      }

      [Fact]
      public async Task OnGet_RequestSent_ExposesRequestedDate()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel
         {
            Htb = "23/07/2026",
            HasHtbDate = true,
            SfsoCommissioningRequestedDate = DateTime.Today
         };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.True(subject.RequestSent);
         Assert.Equal(DateTime.Today, subject.RequestedDate);
      }

      [Fact]
      public async Task OnGet_RequestDateInPast_IsFlagged()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel
         {
            Htb = "23/07/2026",
            HasHtbDate = true,
            SfsoCommissioningRequestedDate = new DateTime(2020, 7, 23)
         };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.True(subject.RequestSent);
         Assert.True(subject.RequestDateInPast);
      }

      [Fact]
      public async Task OnGet_PrefillsOverview()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel { HasHtbDate = false };
         FoundProjectFromRepo.SfsoCommissioningOverview = "existing overview";
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.Equal("existing overview", subject.SfsoCommissioningOverview);
      }

      [Fact]
      public async Task OnPost_ValidOverview_SavesAndRedirectsToTaskList()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel { HasHtbDate = false };
         var subject = Subject();
         subject.SfsoCommissioningOverview = new string('a', 250);

         var result = await subject.OnPostAsync();

         ProjectRepository.Verify(r => r.UpdateSfsoCommissioning(ProjectUrn0001, new string('a', 250)), Times.Once);
         var redirect = Assert.IsType<RedirectToPageResult>(result);
         Assert.Equal("/Projects/Index", redirect.PageName);
      }

      [Fact]
      public async Task OnPost_OverviewTooLong_DoesNotSave()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel { HasHtbDate = false };
         var subject = Subject();
         subject.SfsoCommissioningOverview = new string('a', 251);
         subject.ModelState.AddModelError("SfsoCommissioningOverview", "Overview must be 250 characters or less");

         var result = await subject.OnPostAsync();

         Assert.IsType<PageResult>(result);
         ProjectRepository.Verify(r => r.UpdateSfsoCommissioning(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
      }
   }
}
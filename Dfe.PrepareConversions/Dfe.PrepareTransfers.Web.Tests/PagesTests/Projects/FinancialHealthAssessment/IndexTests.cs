
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

      private FhaIndex Subject() => new(ProjectRepository.Object) { Urn = ProjectUrn0001 };

      [Fact]
      public async Task OnGet_NoHtbDate_NotRequested()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel { HasHtbDate = false };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.False(subject.HasAllMandatoryInformation);
         Assert.False(subject.RequestSent);
      }

      [Fact]
      public async Task OnGet_RequestDateInFuture_RequestWillBeSent()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel
         {
            Htb = "23/07/2026",
            Target = "24/07/2026",
            HasHtbDate = true,
            HasTargetDateForTransfer = true,
            SfsoCommissioningRequestedDate = DateTime.Today.AddDays(5)
         };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.True(subject.HasAllMandatoryInformation);
         Assert.True(subject.RequestWillBeSent);
         Assert.False(subject.RequestSent);
      }

      [Fact]
      public async Task OnGet_RequestDateToday_RequestSent()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel
         {
            Htb = "23/07/2026",
            Target = "24/07/2026",
            HasHtbDate = true,
            HasTargetDateForTransfer = true,
            SfsoCommissioningRequestedDate = DateTime.Today
         };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.True(subject.RequestSent);
         Assert.False(subject.RequestWillBeSent);
         Assert.Equal(DateTime.Today, subject.RequestedDate);
      }

      [Fact]
      public async Task OnGet_RequestDateInPast_RequestSent()
      {
         FoundProjectFromRepo.Dates = new TransferDatesModel
         {
            Htb = "23/07/2026",
            Target = "24/07/2026",
            HasHtbDate = true,
            HasTargetDateForTransfer = true,
            SfsoCommissioningRequestedDate = new DateTime(2020, 7, 23)
         };
         var subject = Subject();

         await subject.OnGetAsync();

         Assert.True(subject.RequestSent);
         Assert.False(subject.RequestWillBeSent);
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
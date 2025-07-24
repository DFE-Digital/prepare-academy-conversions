using System;
using FluentValidation;
using FluentValidation.TestHelper;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Models.TransferDates;
using Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Transfers.Validators.TransferDates;
using Xunit;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Areas.Transfers.Validators.TransferDates;

namespace Dfe.PrepareTransfers.Web.Tests.ValidatorTests.TransferDates
{
    public class PreviouslyConsideredDateValidatorTests
    {
      private readonly PreviouslyConsideredDateValidator _validator;

      public PreviouslyConsideredDateValidatorTests() => _validator = new PreviouslyConsideredDateValidator();

      [Fact]
      public void ShouldHaveChildValidators()
      {
         _validator.ShouldHaveChildValidator(a => a.PreviouslyConsideredDate, typeof(DateValidator));
      }

      [Fact]
      public async Task GivenPreviouslyConsideredDateGreaterThanTargetDate_ShouldGiveError()
      {
         var advisoryBoardDate = DateTime.Now.AddMonths(2);
         var targetDate = DateTime.Now.AddMonths(1);
         var vm = new PreviouslyConsideredViewModel()
         {
            PreviouslyConsideredDate = new DateViewModel
            {
               Date = new DateInputViewModel
               {
                  Day = advisoryBoardDate.Day.ToString(),
                  Month = advisoryBoardDate.Month.ToString(),
                  Year = advisoryBoardDate.Year.ToString(),
               }
            }
         };

         var validationContext = new ValidationContext<PreviouslyConsideredViewModel>(vm)
         {
            RootContextData =
                {
                    ["TargetDate"] = targetDate.ToShortDate()
                }
         };

         var result = await _validator.ValidateAsync(validationContext);
         Assert.Single(result.Errors);
         Assert.Equal("The previously considered date must be on or before the target date for the transfer", result.ToString());
      }

      [Fact]
      public async Task GivenTargetDateGreaterThanPreviouslyConsideredDate_ShouldNotGiveError()
      {
         var previouslyConsideredDate = DateTime.Now.AddMonths(1);
         var targetDate = DateTime.Now.AddMonths(2);
         var vm = new PreviouslyConsideredViewModel
         {
            PreviouslyConsideredDate = new DateViewModel
            {
               Date = new DateInputViewModel
               {
                  Day = previouslyConsideredDate.Day.ToString(),
                  Month = previouslyConsideredDate.Month.ToString(),
                  Year = previouslyConsideredDate.Year.ToString(),
               }
            }
         };

         var validationContext = new ValidationContext<PreviouslyConsideredViewModel>(vm)
         {
            RootContextData =
                {
                    ["TargetDate"] = targetDate.ToShortDate()
                }
         };

         var result = await _validator.ValidateAsync(validationContext);
         Assert.Empty(result.Errors);
      }

      [Theory]
      [InlineData(null)]
      [InlineData("")]
      [InlineData(" ")]
      public async Task GivenPreviouslyConsideredDateAndNoTargetDate_ShouldNotGiveError(string targetDate)
      {
         var advisoryBoardDate = DateTime.Today;
         var vm = new PreviouslyConsideredViewModel()
         {
            PreviouslyConsideredDate = new DateViewModel
            {
               Date = new DateInputViewModel
               {
                  Day = advisoryBoardDate.Day.ToString(),
                  Month = advisoryBoardDate.Month.ToString(),
                  Year = advisoryBoardDate.Year.ToString(),
               }
            }
         };

         var validationContext = new ValidationContext<PreviouslyConsideredViewModel>(vm)
         {
            RootContextData =
                {
                    ["TargetDate"] = targetDate
                }
         };

         var result = await _validator.ValidateAsync(validationContext);
         Assert.Empty(result.Errors);
      }

      [Fact]
      public async Task GivenTargetDateAndUnknownPreviouslyConsideredDate_ShouldNotGiveError()
      {
         var targetDate = DateTime.Now;
         var vm = new PreviouslyConsideredViewModel
         {
            PreviouslyConsideredDate = new DateViewModel
            {
               Date = new DateInputViewModel(),
               UnknownDate = true
            }
         };

         var validationContext = new ValidationContext<PreviouslyConsideredViewModel>(vm)
         {
            RootContextData =
                {
                    ["TargetDate"] = targetDate.ToShortDate()
                }
         };

         var result = await _validator.ValidateAsync(validationContext);
         Assert.Empty(result.Errors);
      }
   }
}

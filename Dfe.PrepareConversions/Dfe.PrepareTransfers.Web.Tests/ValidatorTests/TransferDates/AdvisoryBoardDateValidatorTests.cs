using System;
using FluentValidation;
using FluentValidation.TestHelper;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Models.TransferDates;
using Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Transfers.Validators.TransferDates;
using Xunit;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Tests.ValidatorTests.TransferDates
{
    public class AdvisoryBoardDateValidatorTests
    {
        private readonly AdvisoryBoardDateValidator _validator;

        public AdvisoryBoardDateValidatorTests() => _validator = new AdvisoryBoardDateValidator();
        
        [Fact]
        public void ShouldHaveChildValidators()
        {
            _validator.ShouldHaveChildValidator(a => a.ProposedDecisionDate, typeof(DateValidator));
        }

        [Fact]
        public async Task GivenProposedDecisionDateGreaterThanTargetDate_ShouldGiveError()
        {
            var advisoryBoardDate = DateTime.Now.AddMonths(2);
            var targetDate = DateTime.Now.AddMonths(1);
            var vm = new AdvisoryBoardViewModel()
            {
                ProposedDecisionDate = new DateViewModel
                {
                    Date = new DateInputViewModel
                    {
                        Day = advisoryBoardDate.Day.ToString(),
                        Month = advisoryBoardDate.Month.ToString(),
                        Year = advisoryBoardDate.Year.ToString(),
                    }
                }
            };
            
            var validationContext = new ValidationContext<AdvisoryBoardViewModel>(vm)
            {
                RootContextData =
                {
                    ["TargetDate"] = targetDate.ToShortDate()
                }
            };
            
            var result = await _validator.ValidateAsync(validationContext);
            Assert.Single(result.Errors);
            Assert.Equal("The proposed decision date must be on or before the target date for the transfer", result.ToString());
        }
        
        [Fact]
        public async void GivenTargetDateGreaterThanAdvisoryBoardDate_ShouldNotGiveError()
        {
            var advisoryBoardDate = DateTime.Now.AddMonths(1);
            var targetDate = DateTime.Now.AddMonths(2);
            var vm = new AdvisoryBoardViewModel
            {
                ProposedDecisionDate = new DateViewModel
                {
                    Date = new DateInputViewModel
                    {
                        Day = advisoryBoardDate.Day.ToString(),
                        Month = advisoryBoardDate.Month.ToString(),
                        Year = advisoryBoardDate.Year.ToString(),
                    }
                }
            };
            
            var validationContext = new ValidationContext<AdvisoryBoardViewModel>(vm)
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
        public async void GivenAdvisoryBoardDateAndNoTargetDate_ShouldNotGiveError(string targetDate)
        {
            var advisoryBoardDate = DateTime.Today;
            var vm = new AdvisoryBoardViewModel()
            {
                ProposedDecisionDate = new DateViewModel
                {
                    Date = new DateInputViewModel
                    {
                        Day = advisoryBoardDate.Day.ToString(),
                        Month = advisoryBoardDate.Month.ToString(),
                        Year = advisoryBoardDate.Year.ToString(),
                    }
                }
            };
            
            var validationContext = new ValidationContext<AdvisoryBoardViewModel>(vm)
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
        public async void GivenTargetDateAndUnknownAdvisoryBoardDate_ShouldNotGiveError()
        {
            var targetDate = DateTime.Now;
            var vm = new AdvisoryBoardViewModel
            {
                ProposedDecisionDate = new DateViewModel
                {
                    Date = new DateInputViewModel(),
                    UnknownDate = true
                }
            };
            
            var validationContext = new ValidationContext<AdvisoryBoardViewModel>(vm)
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
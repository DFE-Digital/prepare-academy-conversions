using Dfe.PrepareTransfers.Data;
using FluentValidation.TestHelper;
using Dfe.PrepareTransfers.Web.Pages.Transfers;
using Dfe.PrepareTransfers.Web.Validators.Transfers;
using Moq;
using Xunit;
using Dfe.PrepareTransfers.Web.Pages.NewTransfer;

namespace Dfe.PrepareTransfers.Web.Tests.ValidatorTests.Transfers
{
    public class OutgoingTrustNameValidatorTests
    {
      private readonly OutgoingTrustNameValidator _validator;

      public OutgoingTrustNameValidatorTests()
      {
         _validator = new OutgoingTrustNameValidator();
      }

      [Theory]
      [InlineData(null)]
      [InlineData("")]
      [InlineData(" ")]
      public async void WhenOutgoingTrustNameIsEmpty_ShouldSetError(string trustName)
      {
         var model = new TrustNameModel()
         {
            SearchQuery = trustName
         };

         var result = await _validator.TestValidateAsync(model);

         result.ShouldHaveValidationErrorFor(x => x.SearchQuery)
             .WithErrorMessage("Enter the outgoing trust name");
      }

      [Fact]
      public async void WhenOutgoingTrustIdIsNotEmpty_ShouldNotSetError()
      {
         var model = new TrustNameModel()
         {
            SearchQuery = "test trust name"
         };
         var result = await _validator.TestValidateAsync(model);

         result.ShouldNotHaveValidationErrorFor(x => x);
      }
   }
}

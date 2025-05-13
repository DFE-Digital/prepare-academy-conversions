using Dfe.PrepareTransfers.Data;
using FluentValidation.TestHelper;
using Dfe.PrepareTransfers.Web.Validators.Transfers;
using Moq;
using Xunit;
using Dfe.PrepareTransfers.Web.Pages.NewTransfer;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Tests.ValidatorTests.Transfers
{
    public class OutgoingTrustNameValidatorTests
    {
      private readonly OutgoingTrustNameValidator _validator;
      private readonly Mock<ITrusts> _trustsRepository;

      public OutgoingTrustNameValidatorTests()
      {
         _validator = new OutgoingTrustNameValidator();
         _trustsRepository = new Mock<ITrusts>();
      }

      [Theory]
      [InlineData(null)]
      [InlineData("")]
      [InlineData(" ")]
      public async Task WhenOutgoingTrustNameIsEmpty_ShouldSetError(string trustName)
      {
         var model = new TrustNameModel(_trustsRepository.Object)
         {
            SearchQuery = trustName
         };

         var result = await _validator.TestValidateAsync(model);

         result.ShouldHaveValidationErrorFor(x => x.SearchQuery)
             .WithErrorMessage("Enter the outgoing trust name");
      }

      [Fact]
      public async Task WhenOutgoingTrustIdIsNotEmpty_ShouldNotSetError()
      {
         var model = new TrustNameModel(_trustsRepository.Object)
         {
            SearchQuery = "test trust name"
         };
         var result = await _validator.TestValidateAsync(model);

         result.ShouldNotHaveValidationErrorFor(x => x);
      }
   }
}

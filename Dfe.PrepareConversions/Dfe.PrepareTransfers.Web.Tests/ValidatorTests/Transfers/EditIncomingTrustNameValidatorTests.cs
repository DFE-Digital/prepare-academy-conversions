using Dfe.PrepareTransfers.Data;
using FluentValidation.TestHelper;
using Dfe.PrepareTransfers.Web.Validators.Transfers;
using Moq;
using Xunit;
using Dfe.PrepareTransfers.Web.Pages.Projects.AcademyAndTrustInformation;
using Microsoft.AspNetCore.Http;

namespace Dfe.PrepareTransfers.Web.Tests.ValidatorTests.Transfers
{
    public class EditIncomingTrustNameValidatorTests
    {
        private readonly Mock<IProjects> _projectRepository;
        private Mock<ISession> _session = new Mock<ISession>();
        private readonly EditIncomingTrustNameValidator _validator;

        public EditIncomingTrustNameValidatorTests()
        {
            _validator = new EditIncomingTrustNameValidator();
            _projectRepository = new Mock<IProjects>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void WhenProjectNameIsEmpty_ShouldSetError(string projectName)
        {
            var projectSearch = new IncomingTrustNameModel(_projectRepository.Object, _session.Object)
            {
                IncomingTrustName = projectName
            };
            var result = await _validator.TestValidateAsync(projectSearch);

            result.ShouldHaveValidationErrorFor(x => x.IncomingTrustName)
                .WithErrorMessage("Enter the Incoming trust name");
        }

        [Fact]
        public async void WhenProjectNameNotEmpty_ShouldNotSetError()
        {
            var projectSearch = new IncomingTrustNameModel(_projectRepository.Object, _session.Object)
            {
                IncomingTrustName = "New Project Name"
            };
            var result = await _validator.TestValidateAsync(projectSearch);

            result.ShouldNotHaveValidationErrorFor(x => x);
        }
    }
}

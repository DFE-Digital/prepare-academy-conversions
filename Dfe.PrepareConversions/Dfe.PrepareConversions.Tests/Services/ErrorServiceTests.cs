using Dfe.PrepareConversions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Services
{
	public class ErrorServiceTests
	{
		private readonly ErrorService _errorService = new ErrorService();

		[Fact]
		public void GivenAddError_CanRetrieveError()
		{
			_errorService.AddError("error_key", "error_message");
			_errorService.GetError("error_key").Message.Should().Be("error_message");
		}

		[Fact]
		public void GivenAddTramsError_CanRetrieveTramsError()
		{
			_errorService.AddApiError();
			var errors = _errorService.GetErrors();
			errors.Count().Should().Be(1);
			errors.First().Message.Should().Contain("There is a system problem");
		}

		[Fact]
		public void GivenAddDateError_CanRetrieveDateError()
		{
			var model = new ModelStateDictionary();

			model.AddModelError("deadline", "deadline date should be present");
			model.AddModelError("deadline-day", "deadline date should be present");
			// a date error is defined as any error with key ending in "-day" or "-month" or "-year"
			_errorService.AddErrors(new List<string>{ "deadline-day"}, model);

			var errors =_errorService.GetErrors();
			errors.Count().Should().Be(1);
			errors.First().Key.Should().Be("deadline");
			errors.First().Message.Should().Be("deadline date should be present");
		}

		[Fact]
		public void GivenMultipleErrorsOnSameDateInputField_RetrievesJustOneError()
		{
			var model = new ModelStateDictionary();

			model.AddModelError("deadline", "deadline date should be present");
			_errorService.AddErrors(new List<string> { "deadline-day", "deadline-month" }, model);

			var errors = _errorService.GetErrors();
			errors.Count().Should().Be(1);
			errors.First().Key.Should().Be("deadline");
			errors.First().Message.Should().Be("deadline date should be present");
		}

		[Fact]
		public void GivenDateErrorsWithMultipleModelStateErrorsForOneField_RetrievesOneErrorWithListOfInvalidInputs()
		{
			var model = new ModelStateDictionary();

			model.AddModelError("deadline", "deadline date should be present");
			model.AddModelError("deadline-day", "deadline day should be present");
			model.AddModelError("deadline-month", "deadline month should be present");
			_errorService.AddErrors(new List<string> { "deadline-day", "deadline-month" }, model);

			var errors = _errorService.GetErrors();
			errors.Count().Should().Be(1);
			var invalidInputs = errors.First().InvalidInputs;
			invalidInputs.Count.Should().Be(2);
			invalidInputs.Contains("deadline-day");
			invalidInputs.Contains("deadline-month");
		}

		[Fact]
		public void GivenDateErrorsForMultipleFields_RetrievesOneErrorForEachField()
		{
			var model = new ModelStateDictionary();

			model.AddModelError("deadline", "deadline date should be present");
			_errorService.AddErrors(new List<string> { "deadline-day", "deadline-month" }, model);
			model.AddModelError("start-date", "start date should be present");
			_errorService.AddErrors(new List<string> { "start-date-month", "start-date-year" }, model);

			var errors = _errorService.GetErrors();
			errors.Count().Should().Be(2);

			var deadlineError = errors.Where(x => x.Key == "deadline");
			deadlineError.Count().Should().Be(1);
			deadlineError.First().Message.Should().Be("deadline date should be present");

			var startdateError = errors.Where(x => x.Key == "start-date");
			startdateError.Count().Should().Be(1);
			startdateError.First().Message.Should().Be("start date should be present");
		}

		[Fact]
		public void GivenADateErrorWithValidModelState_DoesNotRetrieveError()
		{
			var model = new ModelStateDictionary();

			_errorService.AddErrors(new List<string> { "deadline-day" }, model);

			var errors = _errorService.GetErrors();
			errors.Count().Should().Be(0);
		}

		[Fact]
		public void GivenKeysWithCorrespondingModelStateInvalid_RetrievesErrors()
		{
			var model = new ModelStateDictionary();
			model.AddModelError("error_field1", "error in field 1");
			model.AddModelError("error_field2", "error in field 2");

			_errorService.AddErrors(new List<string> { "error_field1", "error_field2", "no_error_field" }, model);

			var errors = _errorService.GetErrors();
			errors.Count().Should().Be(2);
			errors.Where(x => x.Key == "error_field1").First().Message.Should().Be("error in field 1");
			errors.Where(x => x.Key == "error_field2").First().Message.Should().Be("error in field 2");
			errors.Where(x => x.Key == "no_error_field").Count().Should().Be(0);
		}
	}
}

using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Services;

public class ErrorServiceTests
{
   private readonly ErrorService _errorService = new();

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
      List<Error> errors = _errorService.GetErrors().ToList();
      errors.Count.Should().Be(1);
      errors.First().Message.Should().Contain("There is a system problem");
   }

   [Fact]
   public void GivenAddDateError_CanRetrieveDateError()
   {
      ModelStateDictionary model = new();

      model.AddModelError("deadline", "deadline date should be present");
      model.AddModelError("deadline-day", "deadline date should be present");
      // a date error is defined as any error with key ending in "-day" or "-month" or "-year"
      _errorService.AddErrors(new List<string> { "deadline-day" }, model);

      List<Error> errors = _errorService.GetErrors().ToList();
      errors.Count.Should().Be(1);
      errors.First().Key.Should().Be("deadline");
      errors.First().Message.Should().Be("Deadline date should be present");
   }

   [Fact]
   public void GivenMultipleErrorsOnSameDateInputField_RetrievesJustOneError()
   {
      ModelStateDictionary model = new();

      model.AddModelError("deadline", "deadline date should be present");
      _errorService.AddErrors(new List<string> { "deadline-day", "deadline-month" }, model);

      var errors = _errorService.GetErrors().ToList();
      errors.Count.Should().Be(1);
      errors.First().Key.Should().Be("deadline");
      errors.First().Message.Should().Be("Deadline date should be present");
   }

   [Fact]
   public void GivenDateErrorsWithMultipleModelStateErrorsForOneField_RetrievesOneErrorWithListOfInvalidInputs()
   {
      ModelStateDictionary model = new();

      model.AddModelError("deadline", "deadline date should be present");
      model.AddModelError("deadline-day", "deadline day should be present");
      model.AddModelError("deadline-month", "deadline month should be present");
      _errorService.AddErrors(new List<string> { "deadline-day", "deadline-month" }, model);

      var errors = _errorService.GetErrors().ToList();
      errors.Count.Should().Be(1);
      List<string> invalidInputs = errors.First().InvalidInputs;
      invalidInputs.Count.Should().Be(2);
      invalidInputs.Contains("deadline-day").Should().BeTrue();
      invalidInputs.Contains("deadline-month").Should().BeTrue();
   }

   [Fact]
   public void GivenDateErrorsForMultipleFields_RetrievesOneErrorForEachField()
   {
      ModelStateDictionary model = new();

      model.AddModelError("deadline", "deadline date should be present");
      _errorService.AddErrors(new List<string> { "deadline-day", "deadline-month" }, model);
      model.AddModelError("start-date", "start date should be present");
      _errorService.AddErrors(new List<string> { "start-date-month", "start-date-year" }, model);

      List<Error> errors = _errorService.GetErrors().ToList();
      errors.Count.Should().Be(2);

      List<Error> deadlineError = errors.Where(x => x.Key == "deadline").ToList();
      deadlineError.Count.Should().Be(1);
      deadlineError.First().Message.Should().Be("Deadline date should be present");

      List<Error> startDateError = errors.Where(x => x.Key == "start-date").ToList();
      startDateError.Count.Should().Be(1);
      startDateError.First().Message.Should().Be("Start date should be present");
   }

   [Fact]
   public void GivenADateErrorWithValidModelState_DoesNotRetrieveError()
   {
      ModelStateDictionary model = new();

      _errorService.AddErrors(new List<string> { "deadline-day" }, model);

      IEnumerable<Error> errors = _errorService.GetErrors();
      errors.Count().Should().Be(0);
   }

   [Fact]
   public void GivenKeysWithCorrespondingModelStateInvalid_RetrievesErrors()
   {
      ModelStateDictionary model = new();
      model.AddModelError("error_field1", "error in field 1");
      model.AddModelError("error_field2", "error in field 2");

      _errorService.AddErrors(new List<string> { "error_field1", "error_field2", "no_error_field" }, model);

      var errors = _errorService.GetErrors().ToList();
      errors.Count.Should().Be(2);
      errors.First(x => x.Key == "error_field1").Message.Should().Be("error in field 1");
      errors.First(x => x.Key == "error_field2").Message.Should().Be("error in field 2");
      errors.Count(x => x.Key == "no_error_field").Should().Be(0);
   }
}

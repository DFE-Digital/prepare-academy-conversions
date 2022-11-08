using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Services
{
	public class ErrorService
	{
		private const string TRAMS_ERROR = "There is a problem with TRAMS and we could not save your changes. Contact <a href=\"mailto:sddservicessupport@education.gov.uk\">sddservicessupport@education.gov.uk</a> if this continues.";
		private readonly List<Error> _errors = new List<Error>();

		public void AddError(string key, string message)
		{
			_errors.Add(new Error
			{
				Key = key,
				Message = message
			});
		 }

		public void AddErrors(IEnumerable<string> keys, ModelStateDictionary modelState)
		{
			foreach (var key in keys)
			{
				if (IsDateInputId(key))
				{
					AddDateError(key, modelState);
				}
				else if (modelState.TryGetValue(key, out var entry) && entry.Errors.Count > 0)
				{
					AddError(key, entry.Errors.Last().ErrorMessage);
				}
			}
		}

		public void AddTramsError()
		{
			_errors.Add(new Error
			{
				Message = TRAMS_ERROR
			});
		}

		public Error GetError(string key)
		{
			return _errors.SingleOrDefault(e => e.Key == key);
		}

		public IEnumerable<Error> GetErrors()
		{
			return _errors;
		}

		public bool HasErrors() => _errors.Count > 0;

		private void AddDateError(string key, ModelStateDictionary modelState)
		{
			if (modelState.TryGetValue(DateInputId(key), out var dateEntry) && dateEntry.Errors.Count > 0)
			{
				var dateError = GetError(DateInputId(key));
				if (dateError == null)
				{
					dateError = new Error
					{
						Key = DateInputId(key),
						Message = dateEntry.Errors.First().ErrorMessage
					};
					_errors.Add(dateError);
				}
				if (modelState.TryGetValue(key, out var entry))
				{
					dateError.InvalidInputs.Add(key);
				}
			}
		}

		private bool IsDateInputId(string id)
		{
			return id.EndsWith("-day") || id.EndsWith("-month") || id.EndsWith("-year");
		}

		private string DateInputId(string id)
		{
			var timeUnit = from item in new[] { "-day", "-month", "-year" }
					where id.EndsWith(item)
					select item;
			
			if (timeUnit.Any()) {
				return id.Substring(0, id.LastIndexOf(timeUnit.First()));
			}
			
			return id;
		}
	}
}

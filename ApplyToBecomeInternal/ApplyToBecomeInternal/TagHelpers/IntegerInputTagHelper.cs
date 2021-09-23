using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("govuk-integer-input", TagStructure = TagStructure.WithoutEndTag)]
	public class IntegerInputTagHelper : InputTagHelperBase
	{
		private readonly ErrorService _errorService;

		public IntegerInputTagHelper(IHtmlHelper htmlHelper, ErrorService errorService) : base(htmlHelper)
		{
			_errorService = errorService;
		}

		[HtmlAttributeName("isMonetary")]
		public bool IsMonetary { get; set; }

		protected override async Task<IHtmlContent> RenderContentAsync()
		{
			if (For.ModelExplorer.ModelType != typeof(int))
			{
				throw new ArgumentException();
			}

			var value = (int)For.Model;
			var model = new IntegerInputViewModel
			{
				Id = Id,
				Name = Name,
				Label = Label,
				Hint = Hint,
				Value = IsMonetary ? value.ToMoneyString() : value.ToString(),
				IsMonetary = IsMonetary
			};

			var error = _errorService.GetError(Name);
			if (error != null)
			{
				model.ErrorMessage = error.Message;
				if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}", out var invalidValue))
				{
					model.Value = invalidValue;
				}
			}

			return await _htmlHelper.PartialAsync("_IntegerInput", model);
		}
	}
}

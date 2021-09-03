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
	[HtmlTargetElement("govuk-decimal-input", TagStructure = TagStructure.WithoutEndTag)]
	public class DecimalInputTagHelper : InputTagHelperBase
	{
		private readonly ErrorService _errorService;

		public DecimalInputTagHelper(IHtmlHelper htmlHelper, ErrorService errorService) : base(htmlHelper)
		{
			_errorService = errorService;
		}

		[HtmlAttributeName("isMonetary")]
		public bool IsMonetary { get; set; }

		protected override async Task<IHtmlContent> RenderContentAsync()
		{
			if (For.ModelExplorer.ModelType != typeof(Decimal?))
			{
				throw new ArgumentException();
			}

			var value = (decimal?)For.Model;
			var model = new DecimalInputViewModel
			{
				Id = Id,
				Name = Name,
				Label = Label,
				Value = IsMonetary ? value?.ToMoneyString() : value.ToSafeString(),
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

			return await _htmlHelper.PartialAsync("_DecimalInput", model);
		}
	}
}

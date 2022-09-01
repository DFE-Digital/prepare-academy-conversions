using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalModelBase : PageModel
	{
		public int Id { get; private set; }

		public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		{
			if (context.HandlerArguments.ContainsKey(nameof(Id)))
				Id = (int)context.HandlerArguments[nameof(Id)];
		}
	}
}

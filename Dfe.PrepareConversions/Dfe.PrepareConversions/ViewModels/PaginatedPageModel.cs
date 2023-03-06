using Dfe.PrepareConversions.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.PrepareConversions.ViewModels;

public abstract class PaginatedPageModel : PageModel, IPagination
{
   protected int PageSize { get; set; } = 10;

   protected abstract ApiV2PagingInfo Paging { get; set; }

   [BindProperty(SupportsGet = true)]
   public int CurrentPage { get; set; } = 1;

   public bool HasPreviousPage => CurrentPage > 1;
   public bool HasNextPage => string.IsNullOrWhiteSpace(Paging?.NextPageUrl) is false;
   public int StartingPage => CurrentPage > 5 ? CurrentPage - 5 : 1;
   public int PreviousPage => CurrentPage - 1;
   public int NextPage => CurrentPage + 1;
}

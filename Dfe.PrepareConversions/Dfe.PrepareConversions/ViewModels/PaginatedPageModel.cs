using Dfe.PrepareConversions.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.PrepareConversions.ViewModels;

public abstract class PaginatedPageModel : PageModel, IPagination
{
   public static int PageSize = 10;

   [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

   protected abstract ApiV2PagingInfo Paging { get; set; }

   public bool HasPreviousPage => CurrentPage > 1;
   public bool HasNextPage => string.IsNullOrWhiteSpace(Paging.NextPageUrl) is false;
   public int StartingPage => CurrentPage > 5 ? CurrentPage - 5 : 1;
   public int PreviousPage => CurrentPage - 1;
   public int NextPage => CurrentPage + 1;
}
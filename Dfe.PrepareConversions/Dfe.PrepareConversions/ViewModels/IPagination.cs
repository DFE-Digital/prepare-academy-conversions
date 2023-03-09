namespace Dfe.PrepareConversions.ViewModels;

public interface IPagination
{
   bool HasPreviousPage { get; }
   bool HasNextPage { get; }

   int StartingPage { get; }
   int PreviousPage { get; }
   int CurrentPage { get; }
   int NextPage { get; }
}
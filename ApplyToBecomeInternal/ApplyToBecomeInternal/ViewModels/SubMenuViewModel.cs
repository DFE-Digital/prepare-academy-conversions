namespace ApplyToBecomeInternal.ViewModels
{
	public class SubMenuViewModel
	{
		public SubMenuViewModel(string projectId, SubMenuPage page)
		{
			ProjectId = projectId;
			Page = page;
		}

		public SubMenuPage Page { get; }
		public string ProjectId { get; }
	}
}
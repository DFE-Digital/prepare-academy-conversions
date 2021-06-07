namespace ApplyToBecomeInternal.Models
{
	public static class Links
	{
		public static class ApplicationForm
		{
			public static LinkItem Index = new LinkItem { BackText = "Back", Page = "/ApplicationForm/Index" };
		}

		public static class ProjectList
		{
			public static LinkItem Index = new LinkItem { BackText = "Back to all conversion projects", Page = "/ProjectList/Index" };
		}

		public static class ProjectNotes
		{
			public static LinkItem Index = new LinkItem { BackText = "Back", Page = "/ProjectNotes/Index" };
			public static LinkItem NewNote = new LinkItem { BackText = "Back", Page = "/ProjectNotes/NewNote" };
		}

		public static class TaskList
		{
			public static LinkItem Index = new LinkItem { BackText = "Back to task list", Page = "/TaskList/Index" };
			public static LinkItem PreviewHTBTemplate = new LinkItem { BackText = "Back", Page = "/TaskList/PreviewHTBTemplate" };
			public static LinkItem GenerateHTBTemplate = new LinkItem { BackText = "Back", Page = "/TaskList/GenerateHTBTemplate" };
		}

		public static class SchoolApplicationForm
		{
			public static LinkItem Index = new LinkItem { BackText = "Back", Page = "/ApplicationForm/Index" };
		}

		public static class Rationale
		{
			public static LinkItem Index = new LinkItem { BackText = "Back", Page = "/TaskList/Rationale" };
		}
	}

	public class LinkItem
	{
		public string Page { get; set; }
		public string BackText { get; set; }
	}
}

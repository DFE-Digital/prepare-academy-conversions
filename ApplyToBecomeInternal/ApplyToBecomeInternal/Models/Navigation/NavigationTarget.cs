namespace ApplyToBecomeInternal.Models.Navigation
{
	public enum NavigationTarget
	{
		[Navigation(content: "Back to all conversion projects", url: "/projectlist")]
		ProjectsList,
		[Navigation(content: "Back", url: "/project-notes/{id}")]
		ProjectNotes,
		[Navigation(content: "Back", url: "/task-list/{id}/preview-headteacher-board-template")]
		PreviewHTBTemplate,
		[Navigation(content: "Back", url: "/task-list/{id}")]
		TaskList

	}
}

namespace ApplyToBecomeInternal.Models.ApplicationForm
{
	public class FormField
	{
		public FormField(string title, string content, FormFieldType type = FormFieldType.Text)
		{
			Title = title;
			Content = content;
			Type = type;
		}

		public string Title { get; }
		public string Content { get; }
		public FormFieldType Type { get; }
	}
}
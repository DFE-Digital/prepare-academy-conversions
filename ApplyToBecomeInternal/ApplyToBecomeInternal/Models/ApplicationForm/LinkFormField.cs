namespace ApplyToBecomeInternal.Models.ApplicationForm
{
	public class LinkFormField : FormField
	{
		public LinkFormField(string title, string content, string url) : base(title, content)
		{
			Url = url;
		}
		
		public string Url { get; }
	}
}
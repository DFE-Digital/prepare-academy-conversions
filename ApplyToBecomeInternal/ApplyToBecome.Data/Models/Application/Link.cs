namespace ApplyToBecome.Data.Models.Application
{
	public class Link
	{
		public Link()
		{

		}

		public Link(string name, string url)
		{
			Url = url;
			Name = name;
		}
		
		public string Url { get; set; }
		public string Name { get; set; }
	}
}
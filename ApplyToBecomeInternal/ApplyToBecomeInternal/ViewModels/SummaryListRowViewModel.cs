namespace ApplyToBecomeInternal.ViewModels
{
	public class SummaryListRowViewModel
	{
		/*public SummaryListRowViewModel(string id, string key, string value, string page = null, string routeId = null, string hiddenText = null, bool altLayout = false)
		{
			Id = id;
			Key = key;
			Value = value;
			Page = page;
			RouteId = routeId;
			HiddenText = hiddenText;
			AltLayout = altLayout;
		}*/

		public string Id { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
		public bool IsEmpty => string.IsNullOrWhiteSpace(Value);
		public string Page { get; set; }
		public string RouteId { get; set; }
		public string HiddenText { get; set; }
		public bool AltLayout { get; set; }
	}
}

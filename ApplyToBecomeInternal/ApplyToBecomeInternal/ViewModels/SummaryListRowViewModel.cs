namespace ApplyToBecomeInternal.ViewModels
{
	public class SummaryListRowViewModel
	{
		public SummaryListRowViewModel(string id, string key, string value, string page, string routeId, string hiddenText)
		{
			Id = id;
			Key = key;
			Value = value;
			Page = page;
			RouteId = routeId;
			HiddenText = hiddenText;
		}

		public string Id { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
		public bool IsEmpty => string.IsNullOrWhiteSpace(Value);
		public string Page { get; set; }
		public string RouteId { get; set; }
		public string HiddenText { get; set; }
	}
}

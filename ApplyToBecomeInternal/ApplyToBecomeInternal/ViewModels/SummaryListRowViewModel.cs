namespace ApplyToBecomeInternal.ViewModels
{
	public class SummaryListRowViewModel
	{
		public SummaryListRowViewModel(string key, string value, string page, string routeId, string hiddenText)
		{
			Key = key;
			Value = value;
			Page = page;
			RouteId = routeId;
			HiddenText = hiddenText;
		}

		public string Key { get; set; }
		public string Value { get; set; }
		public bool IsEmpty => string.IsNullOrWhiteSpace(Value);
		public string Page { get; set; }
		public string RouteId { get; set; }
		public string HiddenText { get; set; }
	}
}

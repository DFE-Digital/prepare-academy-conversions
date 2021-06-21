namespace ApplyToBecomeInternal.ViewModels
{
	public class SummaryListRowViewModel
	{
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

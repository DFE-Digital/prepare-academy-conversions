namespace ApplyToBecomeInternal.ViewModels
{
	public class CheckboxInputViewModel
	{
		public CheckboxInputViewModel(string id, string name, bool value, string label)
		{
			Id = id;
			Name = name;
			Value = value;
			Label = label;
		}

		public string Id { get; }
		public string Name { get; }
		public bool Value { get; }
		public string Label { get; }
	}
}

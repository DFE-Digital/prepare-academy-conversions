using System;

namespace ApplyToBecomeInternal.ViewModels
{
	public class DateInputViewModel
	{
		public DateInputViewModel(string id, string name, DateTime? value, string title, string hint)
		{
			Id = id;
			Name = name;
			Value = value;
			Title = title;
			Hint = hint;
		}

		public string Id { get; }
		public string Name { get; }
		public DateTime? Value { get; }
		public string Title { get; }
		public string Hint { get; }
	}
}

using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models
{
	public class Error
	{
		public string Key { get; set; }
		public string Message { get; set; }
		public string AttemptedValue { get; set; }
		public Dictionary<string, AttemptedValue> AttemptedValues = new Dictionary<string, AttemptedValue>();	
	}

	public class AttemptedValue
	{
		public string Value { get; set; }
		public bool IsInvalid { get; set; }
	}
}

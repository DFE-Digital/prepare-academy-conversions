using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm
{
	public class FormSubSection
	{
		public FormSubSection(string heading, IEnumerable<FormField> fields)
		{
			Heading = heading;
			Fields = fields;
		}

		public string Heading { get; }
		public IEnumerable<FormField> Fields { get; }
	}
}
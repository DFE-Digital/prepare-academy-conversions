namespace ApplyToBecome.Data.Models.Application
{
	public class Declaration
	{
		public Declaration(bool signed, string signee)
		{
			Signed = signed;
			Signee = signee;
		}

		public bool Signed { get; }
		public string Signee { get; }
	}
}
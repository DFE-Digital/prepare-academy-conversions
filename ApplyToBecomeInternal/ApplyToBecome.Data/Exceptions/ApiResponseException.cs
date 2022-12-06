using System;

namespace ApplyToBecome.Data.Exceptions
{
	[Serializable]
	public class ApiResponseException : Exception
	{
		public ApiResponseException(string message) : base(message)
		{
		}
	}
}

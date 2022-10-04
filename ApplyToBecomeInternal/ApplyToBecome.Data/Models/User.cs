using System;

namespace ApplyToBecome.Data.Models
{
	public class User
	{
		public User(string id, string emailAddress, string firstName, string lastName)
		{
			Id = id;
			EmailAddress = emailAddress;
			FirstName = firstName;
			LastName = lastName;
		}

		public string Id { get; set; }
		public string EmailAddress { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
	}
}
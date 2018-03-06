using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Test.Common
{
	public class TestUser
	{
		public int TestUserId { get; set; }
		[DisplayName("FirstName")]
		[Required]
		public string FirstName { get; set; }
		[DisplayName("Last Name")]
		[Required]
		public string LastName { get; set; }
		[DisplayName("Email Address")]
		public string EmailAddress { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
	}
}
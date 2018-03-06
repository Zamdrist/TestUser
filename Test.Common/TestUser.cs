using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Test.Common
{
	public class TestUser
	{
		public int TestUserId { get; set; }
		[DisplayName("First Name")]
		[Required]
		public string FirstName { get; set; }
		[DisplayName("Last Name")]
		[Required]
		public string LastName { get; set; }
		[DisplayName("Email Address")]
		[Required]
		public string EmailAddress { get; set; }
	}
}
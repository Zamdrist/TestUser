using System.Collections.Generic;
using Test.Common;
using Test.DataAccess;

namespace Test.BusinessLogic
{
	public class TestUserBusinessLogic
	{
		public List<TestUser> GetTestUsers()
		{
			return TestUserDataAccess.GetTestUsers();
		}

		public TestUser GetTestUser(int testUserId)
		{
			return TestUserDataAccess.GetTestUser(testUserId);
		}

		public TestUser AddUpdateTestUser(TestUser testUser)
		{
			return TestUserDataAccess.AddUpdateTestUser(testUser);
		}

		public void CreateTable()
		{
			TestUserDataAccess.CreateTable();
		}

		public void DeleteTestUser(int testUserId)
		{
			TestUserDataAccess.DeleteTestuser(testUserId);
		}
	}
}
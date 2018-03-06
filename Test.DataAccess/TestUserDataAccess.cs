using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;
using Test.Common;

namespace Test.DataAccess
{
	public static class TestUserDataAccess
	{
		public static void CreateDatabaseAndTables()
		{
			using (var engine = new SqlCeEngine($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\test.sdf\"; Password=\"test_password\""))
			{
				if (!engine.Verify())
				{
					engine.CreateDatabase();
					using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\test.sdf\"; Password=\"test_password\""))
					{
						connection.Execute(@"CREATE TABLE TestUser(
												[TestUserId] [INT] IDENTITY(1,1) PRIMARY KEY NOT NULL,
												[FirstName] [NVARCHAR](256) NOT NULL,
												[LastName] [NVARCHAR](256) NOT NULL,
												[EmailAddress] [NVARCHAR](256) NOT NULL,
												[CreatedDate] [DATETIME]  NOT NULL DEFAULT(GETDATE()),
												[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()))");
					}
				}
			}
		}

		public static List<TestUser> GetTestUsers()
		{
			using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\test.sdf\"; Password=\"test_password\""))
			{
				return connection.Query<TestUser>("SELECT * TestUser").ToList();
			}
		}

		public static TestUser GetTestUser(int testUserId)
		{
			using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\test.sdf\"; Password=\"test_password\""))
			{
				return connection.Query<TestUser>("SELECT * FROM TestUser", new {TestUserId = testUserId}).Single();
			}
		}

		public static TesstUser AddUpdateTestUser(TestUser testUser)
		{
			using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\test.sdf\"; Password=\"test_password\""))
			{
				if (testUser.TestUserId == 0)
				{
					connection.Execute(@"INSERT INTO TestUser
										( 
											[FirstName]
											,[LastName]
											,[EmailAddress] 
										)
										VALUES  
										( 
											@FirstName
											,@LastName
											,@EmailAddress
										)"
						, new { testUser.TestUserId, testUser.FirstName, testUser.LastName, testUser.EmailAddress });

					testUser.TestUserId = connection.ExecuteScalar<int>("SELECT MAX([TestUserId]) FROM TestUser");
				}
				else
				{
					connection.Execute("UPDATE TestUser
										SET [FirstName] = @FirstName
											,[LastName] = @LastName
											,[EmailAddress] = @EmailAddress
											,[ModifiedDate] = GETDATE()
										WHERE [TestUserId] = @TestUserId"
						, new { testUser.TestUserId, testUser.FirstName, testUser.LastName, testUser.EmailAddress });
				}

				return GetTestUser(testUser.TestUserId);

			}
		}
	}
}
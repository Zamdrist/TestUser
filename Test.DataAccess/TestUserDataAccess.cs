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
		private static string connectionString = $"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\test.sdf\"; Password=\"test_password\"";

		private static SqlCeConnection ReturnSqlCeConnection()
		{
			return new SqlCeConnection(TestUserDataAccess.connectionString);
		}

		public static void CreateDatabaseAndTables()
		{
			using (var engine = new SqlCeEngine(TestUserDataAccess.connectionString))
			{
				if (!engine.Verify())
				{
					engine.CreateDatabase();
					using (var connection = TestUserDataAccess.ReturnSqlCeConnection())
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
			using (var connection = TestUserDataAccess.ReturnSqlCeConnection())
			{
				return connection.Query<TestUser>("SELECT * FROM TestUser").ToList();
			}
		}

		public static TestUser GetTestUser(int testUserId)
		{
			using (var connection = TestUserDataAccess.ReturnSqlCeConnection())
			{
				return connection.Query<TestUser>("SELECT * FROM TestUser", new {TestUserId = testUserId}).Single();
			}
		}

		public static TestUser AddUpdateTestUser(TestUser testUser)
		{
			using (var connection = TestUserDataAccess.ReturnSqlCeConnection())
			{
				if (testUser.TestUserId == 0)
				{
					connection.Execute(@"INSERT INTO TestUser
										( 
											[FirstName]
											,[LastName]
											,[EmailAddress]
											,[CreateDate]
										)
										VALUES  
										( 
											@FirstName
											,@LastName
											,@EmailAddress
											,GETDATE()
										)"
						, new { testUser.TestUserId, testUser.FirstName, testUser.LastName, testUser.EmailAddress });

					testUser.TestUserId = connection.ExecuteScalar<int>("SELECT MAX([TestUserId]) FROM TestUser");
				}
				else
				{
					connection.Execute(@"UPDATE TestUser
										SET [FirstName] = @FirstName
											,[LastName] = @LastName
											,[EmailAddress] = @EmailAddress
											,[ModifiedDate] = GETDATE()
										WHERE [TestUserId] = @TestUserId"
						, new { testUser.TestUserId, testUser.FirstName, testUser.LastName, testUser.EmailAddress });
				}

				return TestUserDataAccess.GetTestUser(testUser.TestUserId);
			}
		}
	}
}
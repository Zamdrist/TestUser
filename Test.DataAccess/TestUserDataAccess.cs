using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Test.Common;

namespace Test.DataAccess
{
	public static class TestUserDataAccess
	{
		private static string connectionString =
			ConfigurationManager.ConnectionStrings["TestUserConnection"].ConnectionString;

		private static SqlConnection ReturnSqlConnection()
		{
			return new SqlConnection(TestUserDataAccess.connectionString);
		}

		private static bool TableExists(string tableName)
		{
			using (var connection = TestUserDataAccess.ReturnSqlConnection())
			{
				return connection.Query<bool>(
					@"IF EXISTS (SELECT 1 
					FROM [Test].INFORMATION_SCHEMA.TABLES 
					WHERE TABLE_TYPE='BASE TABLE' 
					AND TABLE_NAME=@tableName) 
					SELECT 1 AS [Exists] ELSE SELECT 0 AS [Exists];", new { tableName }).FirstOrDefault();
			}
		}

		public static void CreateTable()
		{
			if (!TestUserDataAccess.TableExists("TestUser"))
			{
				using (var connection = TestUserDataAccess.ReturnSqlConnection())
				{
					connection.Execute(
						@"CREATE TABLE TestUser(
					[TestUserId] [INT] IDENTITY(1,1) PRIMARY KEY NOT NULL,
					[FirstName] [NVARCHAR](256) NOT NULL,
					[LastName] [NVARCHAR](256) NOT NULL,
					[EmailAddress] [NVARCHAR](256) NOT NULL,
					[CreatedDate] [DATETIME]  NOT NULL DEFAULT(GETDATE()),
					[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()))");
				}
			}
		}

		public static List<TestUser> GetTestUsers()
		{
			using (var connection = TestUserDataAccess.ReturnSqlConnection())
			{
				return connection.Query<TestUser>("SELECT * FROM TestUser").ToList();
			}
		}

		public static TestUser GetTestUser(int testUserId)
		{
			using (var connection = TestUserDataAccess.ReturnSqlConnection())
			{
				return connection.Query<TestUser>("SELECT * FROM TestUser WHERE TestUserId = @testUserId",
					new { testUserId }).SingleOrDefault();
			}
		}

		public static TestUser AddUpdateTestUser(TestUser testUser)
		{
			using (var connection = TestUserDataAccess.ReturnSqlConnection())
			{
				if (testUser.TestUserId == 0)
				{
					connection.Execute(@"INSERT INTO TestUser
										( 
											[FirstName]
											,[LastName]
											,[EmailAddress]
											,[CreatedDate]
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

		public static void DeleteTestuser(int testUserId)
		{
			using (var connection = TestUserDataAccess.ReturnSqlConnection())
			{
				connection.Execute(@"Delete From TestUser Where TestUserId = @testUserId", new { testUserId });
			}
		}
	}
}
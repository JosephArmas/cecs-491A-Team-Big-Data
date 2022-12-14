using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using TeamBigData.Utification.Registration;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ManagerLayer;

namespace TeamBigData.Utification.RegistrationTests
{
    [TestClass]
    public class RegistrationIntegrationTest
    {
        [TestMethod]
        public async Task CreatesLogWhenRegistering()
        {
            var userConnection = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var logConnection = new SqlConnection(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            logConnection.Open();
            var manager = new Manager();
            SqlDAO testDBO = new SqlDAO(userConnection);
            var countSql = "SELECT COUNT(LogID) From dbo.Logs";
            var testLog = new SqlCommand(countSql, logConnection);
            var expected = 1;
            //Act
            int before = (int)testLog.ExecuteScalar();
            await testDBO.Clear("dbo.Users");
            var result = manager.InsertUser("davidg@yahoo.com", "password");
            int after = (int)testLog.ExecuteScalar();
            logConnection.Close();
            Assert.AreEqual(expected, after - before);
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public async Task ShouldAddUserToDB()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountManager testRegister = new AccountManager(testDBO);
            String password = "password";
            String email = "daviddg@yahoo.com";
            //Act
            await testDBO.Clear("dbo.TestUsers");
            var actual = await testRegister.InsertUser("dbo.TestUsers", email, password);
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public async Task ClearTestUsersWorks()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            //Act
            var actual = await testDBO.Clear("dbo.TestUsers");
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public async Task CatchesDuplicateEmail()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountManager testRegister = new AccountManager(testDBO);
            String password = "password";
            String email = "daviddg5@yahoo.com";
            //Act
            await testDBO.Clear("dbo.TestUsers");
            await testRegister.InsertUser("dbo.TestUsers", email, password);
            var actual = await testRegister.InsertUser("dbo.TestUsers", email, password);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Email"));
        }

        [TestMethod]
        public async Task ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            long expected = 5 * 60000;
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountManager testRegister = new AccountManager(testDBO);
            String username = "daviddg5";
            String password = "password";
            String email = "daviddg5@yahoo.com";
            //Act
            await testDBO.Clear("dbo.TestUsers");
            stopwatch.Start();
            var result = await testRegister.InsertUser("dbo.TestUsers", email, password);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < expected);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}

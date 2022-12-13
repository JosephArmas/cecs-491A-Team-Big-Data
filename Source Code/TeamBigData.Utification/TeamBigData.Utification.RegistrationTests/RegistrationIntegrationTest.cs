using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ManagerLayer;
using Azure.Identity;
using TeamBigData.Utification.Security;

namespace TeamBigData.Utification.RegistrationTests
{
    [TestClass]
    public class RegistrationIntegrationTest
    {
        [TestMethod]
        public async Task CreatesLogWhenRegistering()
        {
            //Arrange
            var userConnection = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var logConnection = @"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=False";
            var manager = new Manager();
            SqlDAO testDBO = new SqlDAO(userConnection);
            SqlDAO logDBO = new SqlDAO(logConnection);
            var expected = 1;
            //Act
            int before = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            await testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var result = manager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            int after = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            //Assert
            Assert.AreEqual(expected, after - before);
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public async Task ShouldAddUserToDB()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Testing;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            var manager = new Manager();
            //Act
            await testDBO.DeleteUser(new UserProfile("daviddg@yahoo.com"));
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var actual = manager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public async Task CatchesDuplicateEmail()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Testing;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            var manager = new Manager();
            //Act
            await testDBO.DeleteUser(new UserProfile("daviddg5@yahoo.com"));
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            manager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            var actual = manager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Email"));
        }

        [TestMethod]
        public async Task ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            long expected = 5 * 1000;
            var manager = new Manager();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Testing;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            String password = "password";
            String email = "daviddg5@yahoo.com";
            //Act
            await testDBO.DeleteUser(new UserProfile("daviddg5@yahoo.com"));
            stopwatch.Start();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var result = manager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < expected);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}

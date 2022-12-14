using System.Diagnostics;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utitification.SQLDataAccess;

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
            var securityManager = new SecurityManager();
            SqlDAO testDBO = new SqlDAO(userConnection);
            SqlDAO logDBO = new SqlDAO(logConnection);
            var expected = 1;
            //Act
            int before = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            await testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var result = securityManager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            int after = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            //Assert
            Assert.AreEqual(expected, after - before);
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public async Task ShouldAddUserToDB()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            var securityManager = new SecurityManager();
            //Act
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var actual = securityManager.InsertUser("testUser2@yahoo.com", encryptedPassword, encryptor);
            testDBO.DeleteUser(new UserProfile("testUser2@yahoo.com"));
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public async Task CatchesDuplicateEmail()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            var securityManager = new SecurityManager();
            //Act
            await testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            securityManager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            var actual = securityManager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Email"));
        }

        [TestMethod]
        public async Task ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            long expected = 5 * 1000;
            var securityManager = new SecurityManager();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            //Act
            await testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            stopwatch.Start();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var result = securityManager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            stopwatch.Stop();
            testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < expected);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}

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
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using Azure.Identity;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

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
            var manager = new SecurityManager();
            SqlDAO testDBO = new SqlDAO(userConnection);
            SqlDAO logDBO = new SqlDAO(logConnection);
            IDBSelecter selectDBO = new SqlDAO(userConnection);
            var expected = 1;
            //Act
            int before = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            UserAccount userAccount = testDBO.SelectUserAccount("test@yahoo.com");
            if (userAccount._username == "")
            {
                await testDBO.DeleteUserProfile((new UserProfile(userAccount._userID))._userID);
            }
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var result = manager.InsertUser("test@yahoo.com", encryptedPassword, encryptor);
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
            var manager = new SecurityManager();
            //Act
            UserAccount userAccount = testDBO.SelectUserAccount("disabledUser@yahoo.com");
            if (userAccount._username == "")
            {
                await testDBO.DeleteUserProfile((new UserProfile(userAccount._userID))._userID);
            }
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var actual = manager.InsertUser("disabledUser@yahoo.com", encryptedPassword, encryptor);
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
            var manager = new SecurityManager();
            //Act
            UserAccount userAccount = testDBO.SelectUserAccount(("testUser@yahoo.com"));
            if (userAccount._username == "")
            {
                await testDBO.DeleteUserProfile((new UserProfile(userAccount._userID))._userID);
            }
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
            var manager = new SecurityManager();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            //Act
            UserAccount userAccount = testDBO.SelectUserAccount(("testUser@yahoo.com"));
            if (userAccount._username == "")
            {
                await testDBO.DeleteUserProfile((new UserProfile(userAccount._userID))._userID);
            }
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

using TeamBigData.Utification.SQLDataAccess;
using System.Diagnostics;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.RecoveryTests
{
    [TestClass]
    public class AccountRecoveryUnitTest
    {
        /*[TestMethod]
        public async Task RequestAvailableWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            SecurityManager secManagerAccRecovery = new SecurityManager();
            long expected = 5 * 1000;
            var userProfile = new UserProfile(new GenericIdentity("username", "Admin User"));
            var requestDB = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Users;Integrated Security = true;TrustServerCertificate=True;Encrypt=True");
            var secManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var newPassword = "password";
            var encryptedPassword = encryptor.encryptString(newPassword);
            //Act
            //Insert Request and Check if its Available
            stopwatch.Start();
            secManager.GenerateOTP();
            var otp = secManager.SendOTP();
            Response insertResult = await secManager.RecoverAccount(username, encryptedPassword, encryptor);
            var fetchResult = await secManagerAccRecovery.GetRecoveryRequests(userProfile);
            Console.WriteLine(fetchResult.IsSuccessful);
            var listRequests = fetchResult.Data;
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            Assert.IsNotNull(listRequests);
            Assert.IsTrue(listRequests.Count > 0);
            Assert.IsTrue(insertResult.IsSuccessful && fetchResult.IsSuccessful);
            Assert.AreEqual(insertResult.ErrorMessage, "Account recovery request sent");
            Assert.IsTrue(actual < expected);
        }

        [TestMethod]
        public async Task AdminApprovedAccountRecovery()
        {
            //Arrange
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var newPassword = "password";
            var encryptedPassword = encryptor.encryptString(newPassword);
            var requestDB = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Users;Integrated Security = true;TrustServerCertificate=True;Encrypt=True");
            SecurityManager adminManager = new SecurityManager();
            var userProfile = new UserProfile(new GenericIdentity("username", "Admin User"));
            var stopwatch = new Stopwatch();
            long expected = 5 * 1000;
            // Create Recovery REquest so admin can finish it
            Response insertResult = await adminManager.RecoverAccount(username, encryptedPassword, encryptor);
            Console.WriteLine(insertResult.IsSuccessful);
            // Admin gets all the requests
            var getResponse = await adminManager.GetRecoveryRequests(userProfile);
            Console.WriteLine(getResponse.IsSuccessful);
            Console.WriteLine(getResponse.ErrorMessage);
            var list = getResponse.Data;
            //Act
            stopwatch.Start();
            var enableResponse = await adminManager.ResetAccount(list[0].UserID, userProfile);
            Console.WriteLine(enableResponse.ErrorMessage);
            stopwatch.Stop();
            long actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(enableResponse.IsSuccessful);
            Assert.AreEqual(enableResponse.ErrorMessage, "Account recovery completed successfully for user");
            Assert.IsTrue(actual < expected);
        }

        [TestMethod]
        public async Task InvalidUsernameFails()
        {
            //Arrange
            var requestDB = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            SecurityManager adminManager = new SecurityManager();
            var secManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser";
            var newPassword = "password";
            var encryptedPassword = encryptor.encryptString(newPassword);
            var expected = "Invalid username or OTP provided. Retry again or contact system administrator";
            //Act
            secManager.GenerateOTP();
            var otp = secManager.SendOTP();
            var actual = await secManager.RecoverAccount(username, encryptedPassword, encryptor);
            //Assert
            Assert.IsFalse(actual.IsSuccessful);
            Assert.AreEqual(expected, actual.ErrorMessage);
        }

        [TestMethod]
        public async Task InvalidNewPasswordFails()
        {
            //Arrange
            var requestDB = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            SecurityManager adminManager = new SecurityManager();
            var secManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var newPassword = "short";
            var encryptedPassword = encryptor.encryptString(newPassword);
            var expected = "Invalid new password. Please make it at least 8 characters and no weird symbols";
            //Act
            secManager.GenerateOTP();
            var otp = "wrongOTP";
            var actual = await secManager.RecoverAccount(username, encryptedPassword, encryptor);
            //Assert
            Assert.IsFalse(actual.IsSuccessful);
            Assert.AreEqual(expected, actual.ErrorMessage);
        }*/
    }
}
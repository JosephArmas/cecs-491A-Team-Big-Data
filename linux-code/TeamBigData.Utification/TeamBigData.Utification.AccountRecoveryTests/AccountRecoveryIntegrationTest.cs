using TeamBigData.Utification.SQLDataAccess;
using System.Diagnostics;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.AccountRecoveryTests
{
    [TestClass]
    public class AccountRecoveryUnitTest
    {
        private String localconn = @"Server=localhost;port=3306;Database=dbo;Uid=root;Pwd=user;";
        [TestMethod]
        public void RequestAvailableWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            SecurityManager secManagerAccRecovery = new SecurityManager();
            long expected = 5 * 1000;
            var userProfile = new UserProfile(new GenericIdentity("username", "Admin User"));
            var requestDB = new SqlDAO(localconn);
            var secManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var newPassword = "password";
            var encryptedPassword = encryptor.encryptString(newPassword);
            List<string> listRequests = new List<string>();
            //Act
            //Insert Request and Check if its Available
            stopwatch.Start();
            secManager.GenerateOTP();
            var otp = secManager.SendOTP();
            Response insertResult = secManager.RecoverAccount(username, encryptedPassword, encryptor, otp).Result;   
            Response fetchResult = secManagerAccRecovery.GetRecoveryRequests(ref listRequests, userProfile);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            Assert.IsNotNull(listRequests);
            Assert.IsTrue(insertResult.isSuccessful && fetchResult.isSuccessful);
            Assert.AreEqual(insertResult.errorMessage, "Account recovery request sent");
            Assert.IsTrue(actual < expected);
        }

        [TestMethod]
        public void AdminApprovedAccountRecovery()
        {
            //Arrange
            var requestDB = new SqlDAO(localconn);
            SecurityManager adminManager = new SecurityManager();
            var userProfile = new UserProfile(new GenericIdentity("username","Admin User"));
            var stopwatch = new Stopwatch();
            long expected = 5 * 1000;
            //Act
            stopwatch.Start();
            var enableResponse = adminManager.EnableAccount("testUser@yahoo.com", userProfile);
            stopwatch.Stop();
            long actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(enableResponse.isSuccessful);
            Assert.AreEqual(enableResponse.errorMessage, "Account recovery completed successfully for user");
            Assert.IsTrue(actual < expected);
        }

        [TestMethod]
        public void InvalidUsernameFails()
        {
            //Arrange
            var requestDB = new SqlDAO(localconn);
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
            var actual = secManager.RecoverAccount(username, encryptedPassword, encryptor, otp).Result;
            //Assert
            Assert.IsFalse(actual.isSuccessful);
            Assert.AreEqual(expected, actual.errorMessage);
        }

        [TestMethod]
        public void InvalidOTPFails()
        {
            //Arrange
            var requestDB = new SqlDAO(localconn);
            SecurityManager adminManager = new SecurityManager();
            var secManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var newPassword = "password";
            var encryptedPassword = encryptor.encryptString(newPassword);
            var expected = "Invalid username or OTP provided. Retry again or contact system administrator";
            //Act
            secManager.GenerateOTP();
            var otp = "wrongOTP";
            var actual = secManager.RecoverAccount(username, encryptedPassword, encryptor, otp).Result;
            //Assert
            Assert.IsFalse(actual.isSuccessful);
            Assert.AreEqual(expected, actual.errorMessage);
        }

        [TestMethod]
        public void InvalidNewPasswordFails()
        {
            //Arrange
            var requestDB = new SqlDAO(localconn);
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
            var actual = secManager.RecoverAccount(username, encryptedPassword, encryptor, otp).Result;
            //Assert
            Assert.IsFalse(actual.isSuccessful);
            Assert.AreEqual(expected, actual.errorMessage);
        }
    }
}
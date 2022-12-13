using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.ManagerLayer;
using TeamBigData.Utification.Security;
using TeamBigData.Utification.SQLDataAccess;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace TeamBigData.Utification.AuthenticationTests
{
    [TestClass]
    public class AuthenticationIntegrationTests
    {
        [TestMethod]
        public void CanEncryptAndDecrypt()
        {
            //Arrange
            var expected = "password";
            var encryptor = new Encryptor();
            //Act
            var encrypted = encryptor.encryptString("password");
            var result = encryptor.decryptString(encrypted);
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SucessfullyLogin()
        {
            //Arrange
            var result = new Response();
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.VerifyUser(username, digest, encryptor);
            var message = manager.SendOTP();
            var result2 = manager.VerifyOTP(message);
            //Assert
            Assert.IsTrue(result.isSuccessful);
            Assert.IsTrue(manager.IsAuthenticated());
        }

        [TestMethod]
        public void CantLoginWhileAlreadyLoggedIn()
        {
            //Arrange
            var result = new Response();
            var expected = "Error You are already Logged In";
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.VerifyUser(username, digest, encryptor);
            var message = manager.SendOTP();
            var result2 = manager.VerifyOTP(message);
            //Verify User is truly authenticated
            Assert.IsTrue(manager.IsAuthenticated());

            result = manager.VerifyUser(username, digest, encryptor);
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(result.errorMessage, expected);
        }

        [TestMethod]
        public void FailsWhenInvalidCredentials()
        {
            //Arrange
            var result = new Response();
            var expected = "Invalid username or password provided. Retry again or contact system administrator";
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "wrongPassword";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.VerifyUser(username, digest, encryptor);
            var message = manager.SendOTP();
            var result2 = manager.VerifyOTP(message);
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(result.errorMessage, expected);
        }

        [TestMethod]
        public void SuccessfullyLogout()
        {
            //Arrange
            var result = new Response();
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.VerifyUser(username, digest, encryptor);
            var message = manager.SendOTP();
            var result2 = manager.VerifyOTP(message);
            result = manager.LogOut();
            //Assert
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public void CantLogoutWhenNotLoggedIn()
        {
            //Arrange
            var result = new Response();
            var manager = new Manager();
            var expected = "Error you are not logged in";
            //Act
            result = manager.LogOut();
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(expected, result.errorMessage);
        }

        [TestMethod]
        public void FailsWhenWrongOTPEntered()
        {
            //Arrange
            var result = new Response();
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.VerifyUser(username, digest, encryptor);
            var result2 = manager.VerifyOTP("wrongOTP");
            //Assert
            Assert.IsFalse(result2.isSuccessful);
            Assert.IsFalse(manager.IsAuthenticated());
        }

        [TestMethod]
        public void OTPExpiresAfter2Minutes()
        {
            //Arrange
            var result = new Response();
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            var expected = "OTP Expired, Please Authenticate Again";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.VerifyUser(username, digest, encryptor);
            var message = manager.SendOTP();
            Thread.Sleep(120000);//Wait 2 Minutes
            var result2 = manager.VerifyOTP(message);
            //Assert
            Assert.AreEqual(expected, result2.errorMessage);
            Assert.IsFalse(result2.isSuccessful);
            Assert.IsFalse(manager.IsAuthenticated());
        }
    }
}

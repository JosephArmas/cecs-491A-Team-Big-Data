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

namespace TeamBigData.Utification.AuthenticationTests
{
    [TestClass]
    public class AuthenticationIntegrationTests
    {
        [TestMethod]
        public void CanCheckDataStore()
        {
            //Arrange
            var connection = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var dbo = new SqlDAO(connection);
            var expected = 1;
            //Act
            var result = dbo.CountAll("dbo.Users", "username");
            Console.Write(result.Result.errorMessage);
            //Assert
            Assert.IsTrue(result.Result.isSuccessful);
            Assert.AreEqual((int)result.Result.data, expected);
        }

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
            var username = "daviddg@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.AuthenticateUser(username, digest, encryptor);
            //Assert
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public void CantLoginWhileAlreadyLoggedIn()
        {
            //Arrange
            var result = new Response();
            var expected = "Error You are already Logged In";
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "daviddg@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            manager.AuthenticateUser(username, digest, encryptor);
            result = manager.AuthenticateUser(username, digest, encryptor);
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
            var username = "daviddg@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            manager.AuthenticateUser(username, digest, encryptor);
            result = manager.LogOut(username);
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
            result = manager.LogOut("daviddg@yahoo.com");
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(expected, result.errorMessage);
        }

        [TestMethod]
        public void FailsWhenWrongOTPEntered()
        {
            //Arrange
            var result = new Response();
            var expected = "Incorrect OTP entered";
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "daviddg@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.AuthenticateUser(username, digest, encryptor);
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(result.errorMessage, expected);
        }

        [TestMethod]
        public void FailsWhenInvalidCredentials()
        {
            //Arrange
            var result = new Response();
            var expected = "Invalid Username or Password";
            var manager = new Manager();
            var encryptor = new Encryptor();
            var username = "david@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = manager.AuthenticateUser(username, digest, encryptor);
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(result.errorMessage, expected);
        }
    }
}

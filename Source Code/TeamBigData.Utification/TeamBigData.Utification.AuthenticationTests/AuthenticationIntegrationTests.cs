using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;

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
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor);
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            //Assert
            Assert.IsTrue(result.isSuccessful);
            Assert.IsTrue(securityManager.IsAuthenticated());
        }

        [TestMethod]
        public void CantLoginWhileAlreadyLoggedIn()
        {
            //Arrange
            var result = new Response();
            var expected = "Error You are already Logged In";
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor);
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            //Verify User is truly authenticated
            Assert.IsTrue(securityManager.IsAuthenticated());

            result = securityManager.VerifyUser(username, digest, encryptor);
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
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "wrongPassword";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor);
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(result.errorMessage, expected);
        }

        [TestMethod]
        public void SuccessfullyLogout()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor);
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            result = securityManager.LogOut();
            //Assert
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public void CantLogoutWhenNotLoggedIn()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var expected = "Error you are not logged in";
            //Act
            result = securityManager.LogOut();
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(expected, result.errorMessage);
        }

        [TestMethod]
        public void FailsWhenWrongOTPEntered()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor);
            var result2 = securityManager.VerifyOTP("wrongOTP");
            //Assert
            Assert.IsFalse(result2.isSuccessful);
            Assert.IsFalse(securityManager.IsAuthenticated());
        }

        [TestMethod]
        public void OTPExpiresAfter2Minutes()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            var expected = "OTP Expired, Please Authenticate Again";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor);
            var message = securityManager.SendOTP();
            Thread.Sleep(120000);//Wait 2 Minutes
            var result2 = securityManager.VerifyOTP(message);
            //Assert
            Assert.AreEqual(expected, result2.errorMessage);
            Assert.IsFalse(result2.isSuccessful);
            Assert.IsFalse(securityManager.IsAuthenticated());
        }
    }
}

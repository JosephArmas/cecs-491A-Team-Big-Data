using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.AuthenticationTests
{
    [TestClass]
    public class AuthenticationUnitTests
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
        public void FailsWhenInvalidCredentials()
        {
            //Arrange
            var result = new Response();
            var expected = "Invalid username or password provided. Retry again or contact system administrator";
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "wrongPassword";
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.LoginUser(username, digest, encryptor, ref userAccount, ref userProfile).Result;
            var message = securityManager.SendOTP();
            var result2 = securityManager.LoginOTP(message);
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(result.errorMessage, expected);
        }

        [TestMethod]
        public void OTPExpiresAfter2Minutes()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var expected = "OTP Expired, Please Authenticate Again";
            //Act
<<<<<<<< HEAD:SourceCode/back-end/TeamBigData.Utification.AuthenticationTests/AuthenticationUnitTest.cs
            var digest = encryptor.encryptString(password);
            result = securityManager.LoginUser(username, digest, encryptor, ref userAccount, ref userProfile).Result;
            securityManager.GenerateOTP();
            var otp = securityManager.SendOTP();
            Thread.Sleep(125000);//Wait 2 Minutes
            result = userAccount.VerifyOTP(otp);
========
            securityManager.GenerateOTP();
            var otp = securityManager.SendOTP();
            Thread.Sleep(125000);//Wait 2 Minutes
            result = securityManager.VerifyOTP(otp);
>>>>>>>> integration:SourceCode/back-end/TeamBigData.Utification.LoginTests/AuthenticationUnitTest.cs
            //Assert
            Assert.AreEqual(expected, result.errorMessage);
            Assert.IsFalse(result.isSuccessful);
        }
    }
}
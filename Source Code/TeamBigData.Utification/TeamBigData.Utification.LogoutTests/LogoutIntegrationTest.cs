using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;

namespace TeamBigData.Utification.LogoutTests
{
    [TestClass]
    public class LogoutUnitTest
    {
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
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
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
    }
}
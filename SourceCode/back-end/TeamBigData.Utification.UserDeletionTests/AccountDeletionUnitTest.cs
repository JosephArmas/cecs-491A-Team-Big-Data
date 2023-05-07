using System.Security.Principal;
using System.Globalization;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.UserDeletionTests
{
    [TestClass]
    public class AccountDeletionUnitTest
    {
       /*
        [TestMethod]
        public void FailsWhenRegularUserTriesToDeleteAnotherUser()
        {
            //Arrange
            var regUser = new UserProfile(7777, "", "", "", DateTime.UtcNow, new GenericIdentity("Regular User")); //Create first Regular user to attempt deletion from
            var vicUser = new UserProfile(7778, "", "", "", DateTime.UtcNow, new GenericIdentity("Regular User")); //Create second Regular user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(vicUser, regUser);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        [TestMethod]
        public void FailsWhenRegularUserTriesToDeleteAdmin()
        {
            //Arrange
            var regUser = new UserProfile(7779, "", "", "", DateTime.UtcNow, new GenericIdentity("Regular User")); //Create Regular user to attempt deletion from
            var adUser = new UserProfile(7780, "", "", "", DateTime.UtcNow, new GenericIdentity("Admin User")); //Create Admin user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(adUser, regUser);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        [TestMethod]
        public void CorrectMessageDisplayed()
        {
            //Arrange
            var regUser = new UserProfile(7781, "", "", "", DateTime.UtcNow, new GenericIdentity("Regular User")); //Create Regular user to attempt deletion from
            var adUser = new UserProfile(7777, "", "", "", DateTime.UtcNow, new GenericIdentity("Admin User")); //Create Admin user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(adUser, regUser);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ErrorMessage == "User does not have permission to delete the account");
        }
       */
    }
}
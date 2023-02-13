using System.Security.Principal;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
namespace TeamBigData.Utification.AccountDeletionTests
{
    [TestClass]
    public class AccountDeletionUnitTest
    {
        [TestMethod]
        public void CanDeleteOwnAccount()
        {
            //Arrange
            var user = new UserProfile(new GenericIdentity("Deletius", "Regular User")); //Create a fake user profile to see if it will allow the deletion
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(user,user); //Start the deletion manager 
            //Assert
            Assert.AreEqual(1,result);
        }
        [TestMethod]
        public void CanAdminDeleteOtherAdminAccount()
        {
            //Arrange
            var adUser = new UserProfile(new GenericIdentity("Kratos", "Admin User")); //Create first admin user to attempt deletion from
            var vicUser = new UserProfile(new GenericIdentity("Ares", "Admin User")); //Create second admin user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(adUser,vicUser);
            //Assert
            Assert.Fail();
        }
        [TestMethod]
        public void CanAdminDeleteUserAccount()
        {
            //Arrange
            var adUser = new UserProfile(new GenericIdentity("God", "Admin User")); //Create admin user to attemot deletion from
            var vicUser = new UserProfile(new GenericIdentity("Sodom", "Regular User")); //Create regular user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(adUser,vicUser);
            //Assert
            Assert.Fail();
        }
        [TestMethod]
        public void FailsWhenRegularUserTriesToDeleteAnotherUser()
        {
            //Arrange
            var regUser = new UserProfile(new GenericIdentity("Cain", "Regular User")); //Create first Regular user to attempt deletion from
            var vicUser = new UserProfile(new GenericIdentity("Abel", "Regular User")); //Create second Regular user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(regUser,vicUser);
            //Assert
            Assert.IsFalse(result.isSuccessful);
        }
        [TestMethod]
        public void FailsWhenRegularUserTriesToDeleteAdmin()
        {
            //Arrange
            var regUser = new UserProfile(new GenericIdentity("Brutus", "Regular User")); //Create Regular user to attempt deletion from
            var adUser = new UserProfile(new GenericIdentity("Caesar", "Admin User")); //Create Admin user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(regUser,adUser);
            //Assert
            Assert.Fail();
        }
        [TestMethod]
        public void DefaultHomeviewDisplayed()
        {
            //Arrange
            //Act
            //Assert
            Assert.Fail();
        }
        [TestMethod]
        public void CorrectMessageDisplayed()
        {
            //Arrange
            //Act
            //Assert
            Assert.Fail();
        }
    }
}
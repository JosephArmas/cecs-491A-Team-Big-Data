using System.Security.Principal;
using System.Globalization;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.View.Views;

namespace TeamBigData.Utification.AccountDeletionTests
{
    [TestClass]
    public class AccountDeletionUnitTest
    {

        [TestMethod]
        public void FailsWhenRegularUserTriesToDeleteAnotherUser()
        {
            //Arrange
            var regUser = new UserProfile(new GenericIdentity("Cain", "Regular User")); //Create first Regular user to attempt deletion from
            var vicUser = new UserProfile(new GenericIdentity("Abel", "Regular User")); //Create second Regular user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(vicUser.Identity.Name,regUser);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
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
            var result = delMan.DeleteAccount(adUser.Identity.Name,regUser);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }
        [TestMethod]
        public void DefaultCultureDisplayed()
        {
            //Arrange
            RegularView view = new RegularView();
            AnonymousView anonView = new AnonymousView();
            //Act
            view.SetCultureInfo(new CultureInfo("fr-FR"));
            //Assert
            Assert.IsFalse(view.culCurrent != anonView.culCurrent);
        }
        [TestMethod]
        public void CorrectMessageDisplayed()
        {
            //Arrange
            var regUser = new UserProfile(new GenericIdentity("Brutus", "Regular User")); //Create Regular user to attempt deletion from
            var adUser = new UserProfile(new GenericIdentity("Caesar", "Admin User")); //Create Admin user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(adUser.Identity.Name, regUser);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.errorMessage == "User does not have permission to delete the account");
        }
    }
}
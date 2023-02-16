using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.AccountDeletionTests
{
    [TestClass]
    public class AccountDeletionIntegrationTest
    {
        [TestMethod]
        public void CanDeleteOwnAccount()
        {
            //Arrange
            var user = new UserProfile(new GenericIdentity("Deletius", "Regular User")); //Create a fake user profile to see if it will allow the deletion
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(user.Identity.Name, user); //Start the deletion manager 
            //Assert
            Console.WriteLine(result.errorMessage + result.isSuccessful);

            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public void CanAdminDeleteOtherAdminAccount()
        {
            //Arrange
            var adUser = new UserProfile(new GenericIdentity("Kratos", "Admin User")); //Create first admin user to attempt deletion from
            var vicUser = new UserProfile(new GenericIdentity("Ares", "Admin User")); //Create second admin user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(vicUser.Identity.Name, adUser);
            //Assert
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public void CanAdminDeleteUserAccount()
        {
            //Arrange
            var adUser = new UserProfile(new GenericIdentity("God", "Admin User")); //Create admin user to attemot deletion from
            var vicUser = new UserProfile(new GenericIdentity("Sodom", "Regular User")); //Create regular user to be deleted
            var delMan = new DeletionManager();
            //Act
            var result = delMan.DeleteAccount(vicUser.Identity.Name, adUser);
            //Assert
            Assert.IsTrue(result.isSuccessful);
        }
    }
}

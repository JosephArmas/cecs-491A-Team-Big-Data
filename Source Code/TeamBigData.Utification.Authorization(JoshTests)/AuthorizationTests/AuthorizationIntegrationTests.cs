using System;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using TeamBigData.Utification.Authorization;
using TeamBigData.Utification.AuthZ.Abstraction;
using TeamBigData.Utification.Authorization.Views;

namespace AuthorizationTests
{
    [TestClass]
    public class AuthorizationIntegrationTests
    {
        [TestMethod]
        public void AnonymousViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount();
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new AnonymousView();
            //Act
            Console.SetIn(new StringReader("1"));
            var logResultAnonymous = view.DisplayMenu(ref userAccount, ref sysUnderTestAnonymous);
            Console.SetIn(new StringReader("1"));
            var logResultAdmin = view.DisplayMenu(ref userAccount, ref sysUnderTestAdmin);
            Console.SetIn(new StringReader("1"));
            var logResultRegular = view.DisplayMenu(ref userAccount, ref sysUnderTestRegular);
            //Assert
            Console.WriteLine("Anonymous User wants to see Anonymouse View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymouse View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymouse View: " + logResultRegular);
            Assert.IsTrue(logResultAnonymous.isSuccessful);
            Assert.IsFalse(logResultAdmin.isSuccessful);
            Assert.IsFalse(logResultRegular.isSuccessful);
        }

        [TestMethod]
        public void AdminViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount();
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new AdminView();
            //Act
            Console.SetIn(new StringReader("1"));
            var logResultAnonymous = view.DisplayMenu(ref userAccount, ref sysUnderTestAnonymous);
            Console.SetIn(new StringReader("1"));
            var logResultAdmin = view.DisplayMenu(ref userAccount, ref sysUnderTestAdmin);
            Console.SetIn(new StringReader("1"));
            var logResultRegular = view.DisplayMenu(ref userAccount, ref sysUnderTestRegular);
            //Assert
            Console.WriteLine("Anonymous User wants to see Anonymouse View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymouse View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymouse View: " + logResultRegular);
            Assert.IsTrue(logResultAdmin.isSuccessful);
            Assert.IsFalse(logResultAnonymous.isSuccessful);
            Assert.IsFalse(logResultRegular.isSuccessful);
        }
        [TestMethod]
        public void RegularViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount();
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new RegularView();
            //Act
            Console.SetIn(new StringReader("1"));
            var logResultAnonymous = view.DisplayMenu(ref userAccount, ref sysUnderTestAnonymous);
            Console.SetIn(new StringReader("1"));
            var logResultAdmin = view.DisplayMenu(ref userAccount, ref sysUnderTestAdmin);
            Console.SetIn(new StringReader("1"));
            var logResultRegular = view.DisplayMenu(ref userAccount, ref sysUnderTestRegular);
            //Assert
            Console.WriteLine("Anonymous User wants to see Anonymouse View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymouse View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymouse View: " + logResultRegular);
            Assert.IsTrue(logResultRegular.isSuccessful);
            Assert.IsFalse(logResultAdmin.isSuccessful);
            Assert.IsFalse(logResultAnonymous.isSuccessful);
        }
        /*
        [TestMethod]
        
        public void UserProfileReflectsUser(UserProfile userProfile)
        {
            //User can only see their personal Profile
        }
        [TestMethod]
        public void RestrictAnonymousViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount();
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new AnonymousView();
            //Act
            Console.SetIn(new StringReader("1"));
            var logResultAnonymous = view.DisplayMenu(ref userAccount, ref sysUnderTestAnonymous);
            Console.SetIn(new StringReader("1"));
            var logResultAdmin = view.DisplayMenu(ref userAccount, ref sysUnderTestAdmin);
            Console.SetIn(new StringReader("1"));
            var logResultRegular = view.DisplayMenu(ref userAccount, ref sysUnderTestRegular);
            //Assert
            Console.WriteLine("Anonymous User wants to see Anonymouse View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymouse View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymouse View: " + logResultRegular);
            Assert.IsTrue(logResultAnonymous);
            Assert.IsFalse(logResultAdmin);
            Assert.IsFalse(logResultRegular);
        }

        [TestMethod]
        public void RestrictAdminViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount();
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new AdminView();
            //Act
            Console.SetIn(new StringReader("1"));
            var logResultAnonymous = view.DisplayMenu(ref userAccount, ref sysUnderTestAnonymous);
            Console.SetIn(new StringReader("1"));
            var logResultAdmin = view.DisplayMenu(ref userAccount, ref sysUnderTestAdmin);
            Console.SetIn(new StringReader("1"));
            var logResultRegular = view.DisplayMenu(ref userAccount, ref sysUnderTestRegular);
            //Assert
            Console.WriteLine("Anonymous User wants to see Anonymouse View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymouse View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymouse View: " + logResultRegular);
            Assert.IsTrue(logResultAdmin);
            Assert.IsFalse(logResultAnonymous);
            Assert.IsFalse(logResultRegular);
        }
        [TestMethod]
        public void RestrictRegularViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount();
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new RegularView();
            //Act
            Console.SetIn(new StringReader("1"));
            var logResultAnonymous = view.DisplayMenu(ref userAccount, ref sysUnderTestAnonymous);
            Console.SetIn(new StringReader("1"));
            var logResultAdmin = view.DisplayMenu(ref userAccount, ref sysUnderTestAdmin);
            Console.SetIn(new StringReader("1"));
            var logResultRegular = view.DisplayMenu(ref userAccount, ref sysUnderTestRegular);
            //Assert
            Console.WriteLine("Anonymous User wants to see Anonymouse View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymouse View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymouse View: " + logResultRegular);
            Assert.IsTrue(logResultRegular);
            Assert.IsFalse(logResultAdmin);
            Assert.IsFalse(logResultAnonymous);
        }
        [TestMethod]
        public void ModificationsAreActive(UserProfile userProfile)
        {
            //Modifications will reflect upon next authentication
        }
        [TestMethod]
        public void AdminPrivilegeDelete(UserProfile userProfile)
        {
            //Admins may delete. may not delete self 
        }
        [TestMethod]
        public void ReputableIsCreated(UserProfile userProfile)
        {
            //Role is elevated when reputation is greater than 4.2 rating
        }
        [TestMethod]
        public void ReputableLosesRole(UserProfile userProfile)
        {
            //Reputable Users are deelevated when reputation falls below 4.2. Upon next authentication
        }
        [TestMethod]
        public void ServiceUserCreated(UserProfile userProfile)
        {
            //User elevated to ServiceUser when profile is updated with Service
        }
        [TestMethod]
        public void ServiceUserViews(UserProfile userProfile)
        {
            //Service User can only see Service Creation view, Map view, Event view,
            //Notification view, and AccessProfile view
        }
        [TestMethod]
        public void ServiceAccessProfile(UserProfile userProfile)
        {
            //Service users can deelivate themselves in AccessProfile view. Must happen upon request
        }
        [TestMethod]
        public void ServiceUserReputation(UserProfile userProfile)
        {
            //Reputation wont changed a ServiceUser's role while a ServiceUser
        }
        [TestMethod]
        public void ReputableViews(UserProfile userProfile)
        {
            //Access to Event Creation and Service Request view
        }
        [TestMethod]
        public void RegularUserCreation(UserProfile userProfile)
        {
            //After creating a UserAccount, the user is no longer anonymous
        }
        [TestMethod]
        public void RegularUserReputation(UserProfile userProfile)
        {
            //RegularUser only has a Regular identity when reputation is below 4.2
        }
        [TestMethod]
        public void RegularUserViews(UserProfile userProfile)
        {
            //Regular users can access LitterMapView, JoinEventView, AlertView,
            //ReputationView, ProfileView, and PictureUploadView
        }
        */

    }
}
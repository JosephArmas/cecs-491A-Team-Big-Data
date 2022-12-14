using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.View.Views;

namespace TeamBigData.Utification.AuthorizationTests
{
    [TestClass]
    public class AuthorizationIntegrationTests
    {
        [TestMethod]
        public void AnonymousViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount("", "");
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new AnonymousView();
            //Act
            Console.SetIn(new StringReader("0"));
            var logResultAnonymous = view.DisplayMenu(ref userAccount, ref sysUnderTestAnonymous);
            Console.SetIn(new StringReader("0"));
            var logResultAdmin = view.DisplayMenu(ref userAccount, ref sysUnderTestAdmin);
            Console.SetIn(new StringReader("0"));
            var logResultRegular = view.DisplayMenu(ref userAccount, ref sysUnderTestRegular);
            //Assert
            bool pass = false;
            if (!logResultAnonymous.isSuccessful && logResultAnonymous.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Anonymous User wants to see Anonymous View: " + pass);
            Assert.IsTrue(pass);
            if (!logResultAdmin.isSuccessful && logResultAdmin.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Admin User wants to see Anonymous View: " + pass);
            Assert.IsFalse(pass);
            if (!logResultRegular.isSuccessful && logResultRegular.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Regular User wants to see Anonymous View: " + pass);
            Assert.IsFalse(pass);
        }

        [TestMethod]
        public void AdminViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount("", "");
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
            Console.WriteLine("Anonymous User wants to see Anonymous View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymous View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymous View: " + logResultRegular);
            Assert.IsTrue(logResultAdmin.isSuccessful);
            Assert.IsFalse(logResultAnonymous.isSuccessful);
            Assert.IsFalse(logResultRegular.isSuccessful);
        }
        [TestMethod]
        public void RegularViewAccess()
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
            //Arrange
            var userAccount = new UserAccount("", "");
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
            Console.WriteLine("Anonymous User wants to see Anonymous View: " + logResultAnonymous);
            Console.WriteLine("Admin User wants to see Anonymous View: " + logResultAdmin);
            Console.WriteLine("Regular User wants to see Anonymous View: " + logResultRegular);
            Assert.IsTrue(logResultRegular.isSuccessful);
            Assert.IsFalse(logResultAdmin.isSuccessful);
            Assert.IsFalse(logResultAnonymous.isSuccessful);
        }
    }
}

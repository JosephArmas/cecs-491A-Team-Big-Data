using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.View.Views;

namespace TeamBigData.Utification.UserManagementTests
{
    [TestClass]
    public class UserManagementIntegrationTests
    {
        [TestMethod]
        public void AdminRestrictedViewAccess()
        {
            //Admin is only allowed to acess UserManagementView
            //Arrange
            var userAccount = new UserAccount();
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new UserManagementView();
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
            Console.WriteLine("Anonymous User wants to see UserManagement View: " + pass);
            Assert.IsFalse(pass);
            if (!logResultAdmin.isSuccessful && logResultAdmin.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Admin User wants to see UserManagement View: " + pass);
            Assert.IsTrue(pass);
            if (!logResultRegular.isSuccessful && logResultRegular.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Regular User wants to see UserManagement View: " + pass);
            Assert.IsFalse(pass);
        }
        [TestMethod]
        public void CreateWithinFiveSeconds()
        {
            //Testing ability to have a task perform under 5 seconds
            //Arrange
            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "";
            SecurityManager securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString(userPassword);

            //Act
            stopwatch.Start();
            response = securityManager.RegisterUser(email, encryptedPassword, encryptor).Result;
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert

            if (actual < expected &&  response.isSuccessful)
            {
                Console.WriteLine("UM was successful");
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [TestMethod]
        public void DeleteWithinFiveSeconds()
        {
            //Testing ability to have a task perform under 5 seconds
            //Arrange
            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "";
            SecurityManager securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString(userPassword);
            var madeUser = securityManager.InsertUser(email, encryptedPassword, encryptor);

            //Act
            stopwatch.Start();
            response = securityManager.DeleteProfile(email, sysUnderTestAdmin);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert

            if (actual < expected && response.isSuccessful)
            {
                Console.WriteLine("UM was successful");
                Assert.IsTrue(true);
            }
            else
                Assert.IsTrue(false);
            }
       
            [TestMethod]
            public void DisableWithinFiveSeconds()
            {
                //Testing ability to have a task perform under 5 seconds
                //Arrange
                Response response = new Response();
                var userAccount = new UserAccount();
                var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
                String userPassword = "Password";
                var stopwatch = new Stopwatch();
                var expected = 5000;
                string email = "";
                SecurityManager securityManager = new SecurityManager();
                var encryptor = new Encryptor();
                var encryptedPassword = encryptor.encryptString(userPassword);
                var madeUser = securityManager.InsertUser(email, encryptedPassword, encryptor);

                //Act
                stopwatch.Start();
                response = securityManager.DisableAccount(email, sysUnderTestAdmin);
                stopwatch.Stop();
                var actual = stopwatch.ElapsedMilliseconds;
            //Assert

            if (actual < expected && response.isSuccessful)
            {
                Console.WriteLine("UM was successful");
                Assert.IsTrue(true);
            }
            else
                Assert.IsTrue(false);
            }

        }
}


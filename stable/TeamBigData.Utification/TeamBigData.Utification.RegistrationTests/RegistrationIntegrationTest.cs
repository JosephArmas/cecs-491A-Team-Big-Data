﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using Azure.Identity;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.Manager.Abstractions;
using System.Security.Cryptography;

namespace TeamBigData.Utification.RegistrationTests
{
    [TestClass]
    public class RegistrationIntegrationTest
    {
        [TestMethod]
        public async Task CreatesLogWhenRegistering()
        {
            //Arrange
            var logConnection = @"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=False";
            IRegister register = new SecurityManager();
            SqlDAO logDBO = new SqlDAO(logConnection);
            var username = "CreateLogWhenRegisteringTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            //Act
            int before = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            var result = register.RegisterUser(username, encryptedPassword, encryptor).Result;
            int after = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            //Assert
            Assert.IsTrue(after > before);
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public async Task ShouldAddUserToDB()
        {
            //Arrange
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;Encrypt=False";
            IDBSelecter testDBO = new SqlDAO(connectionString);
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            var username = "ShouldAddUserToDBTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            //Act
            var test = await register.RegisterUser(username, encryptedPassword, encryptor);
            var expected = await testDBO.SelectUserAccount(ref userAccount, username);
            //Assert
            Assert.IsTrue(username == userAccount._username);
            Assert.IsTrue(test.isSuccessful);
        }

        [TestMethod]
        public async Task CatchesDuplicateEmail()
        {
            //Arrange
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var username = "CatchesDuplicateEmailTest@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            //Act 
            var setup = await register.RegisterUser(username, encryptedPassword, encryptor);
            var actual = await register.RegisterUser(username, encryptedPassword, encryptor);
            //Assert
            Assert.AreEqual(actual.errorMessage, "Email already linked to an account, please pick a new email");
        }

        [TestMethod]
        public async Task ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var username = "ShoudRegisterWithin5SecondsTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            //Act
            stopwatch.Start();
            var result = await register.RegisterUser(username, encryptedPassword, encryptor);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < 5000);
            Assert.IsTrue(result.isSuccessful);
        }
        //Dont know why there are 2
        /*
        [TestMethod]
        public async Task ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            long expected = 5 * 1000;
            var manager = new SecurityManager();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            AccountRegisterer testRegister = new AccountRegisterer(testDBO);
            //Act
            await testDBO.DeleteUser(new UserProfile("testUser@yahoo.com"));
            stopwatch.Start();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var result = manager.InsertUser("testUser@yahoo.com", encryptedPassword, encryptor);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < expected);
            Assert.IsTrue(result.isSuccessful);
        }
        */
    }
}

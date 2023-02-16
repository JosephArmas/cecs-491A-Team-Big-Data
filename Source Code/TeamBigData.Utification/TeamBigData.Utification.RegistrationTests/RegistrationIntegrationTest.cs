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
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile= new UserProfile();
            var username = "CreateLogWhenRegisteringTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(8)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var expected = 1;
            //Act
            int before = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            var result = register.RegisterUser(username, encryptedPassword, encryptor, ref userAccount, ref userProfile);
            int after = (int)logDBO.CountAll("dbo.Logs", "LogID").Result.data;
            //Assert
            Assert.AreEqual(expected, after - before);
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public async Task ShouldAddUserToDB()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBSelecter testDBO = new SqlDAO(connectionString);
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var username = "ShouldAddUserToDBTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(8)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            //Act
            var test = register.RegisterUser(username, encryptedPassword, encryptor, ref userAccount, ref userProfile);
            var expected = testDBO.SelectUserAccount(username);
            //Assert
            Assert.IsTrue(userAccount._username == expected._username);
        }

        [TestMethod]
        public async Task CatchesDuplicateEmail()
        {
            //Arrange
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var username = "CatchesDuplicateEmailTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(8)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            //Act
            register.RegisterUser(username, encryptedPassword, encryptor, ref userAccount, ref userProfile);
            var actual = register.RegisterUser(username, encryptedPassword, encryptor, ref userAccount, ref userProfile);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Email already linked to an account, please pick a new email"));
        }

        [TestMethod]
        public async Task ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var username = "ShoudRegisterWithin5SecondsTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(8)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            //Act
            stopwatch.Start();
            var result = register.RegisterUser(username, encryptedPassword, encryptor, ref userAccount, ref userProfile);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < 5000);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}

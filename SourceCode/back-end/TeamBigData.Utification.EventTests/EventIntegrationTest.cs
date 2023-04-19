using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using Response = TeamBigData.Utification.ErrorResponse.Response;

namespace TeamBigData.Utification.EventTests;

[TestClass]
public class EventIntegrationTest
{
    private readonly DBConnectionString connString = new DBConnectionString();
    [TestMethod]
    public async Task ValidInsert()
    {
        // Arrange
        // var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
        IDBSelecter testDBO = new SqlDAO(connString._connectionStringUsers);
        IRegister register = new SecurityManager();
        var username = "ShouldAddUserToDBTest" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
        var encryptor = new Encryptor();
        var encryptedPassword = encryptor.encryptString("password");
        //Act
        var test = await register.RegisterUser(username, encryptedPassword, encryptor);
        var response = await testDBO.SelectUserAccount(username);
        var expected = response.data;
        var eventManager = new EventManager.EventManager();
        var result = new Response();
        string title = "Beach Club";
        string description = "Beach clean up at Huntington Park";

        // Act
        result = await eventManager.CreateNewEvent(title, description, userID).ConfigureAwait(false);
        
        // Assert
        Assert.IsTrue(result.isSuccessful);
    }
    
}
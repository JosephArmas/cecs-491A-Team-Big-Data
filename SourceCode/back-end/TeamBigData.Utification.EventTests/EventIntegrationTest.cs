using System;
using System.Diagnostics;
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
using TeamBigData.Utification.EventManager;
using TeamBigData.Utification.EventService.Abstractions;
using Response = TeamBigData.Utification.ErrorResponse.Response;

namespace TeamBigData.Utification.EventTests;

[TestClass]
public class EventIntegrationTest
{
    private readonly DBConnectionString connString = new DBConnectionString();

    private async Task<Response> GenerateRegUser()
    {
        IDBSelecter testDBO = new SqlDAO(connString._connectionStringUsers);
        IRegister register = new SecurityManager();
        var username = "TestUser" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
        var encryptor = new Encryptor();
        var encryptedPassword = encryptor.encryptString("password");
        var result = await register.RegisterUser(username, encryptedPassword, encryptor);
        result = await testDBO.SelectUserID(username);
        return result;
    }

    private async Task<Response> GenerateReputableUser()
    {
        
        IDBSelecter testDBO = new SqlDAO(connString._connectionStringUsers);
        IDBUpdater daoUpdate = new SqlDAO(connString._connectionStringUsers);
        IRegister register = new SecurityManager();
        var username = "TestUser" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
        var encryptor = new Encryptor();
        var encryptedPassword = encryptor.encryptString("password");
        var result = await register.RegisterUser(username, encryptedPassword, encryptor);
        var userIDObj = await testDBO.SelectUserID(username);
        
        // Convert the User ID from  type obj to in
        var userID = Convert.ToInt32(userIDObj.data);
        
        await daoUpdate.UpdateUserRole(userID, "Reputable User");
        
        return userIDObj;
    }

    private async Task<Response> GenerateAdminUser()
    {
        IDBSelecter testDBO = new SqlDAO(connString._connectionStringUsers);
        IDBUpdater daoUpdate = new SqlDAO(connString._connectionStringUsers);
        IRegister register = new SecurityManager();
        var username = "AdminUser" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
        var encryptor = new Encryptor();
        var encryptedPassword = encryptor.encryptString("password");
        var result = await register.RegisterUser(username, encryptedPassword, encryptor);
        var userIDObj = await testDBO.SelectUserID(username);
        
        // Convert the User ID from  type obj to in
        var userID = Convert.ToInt32(userIDObj.data);
        
        await daoUpdate.UpdateUserRole(userID, "Admin User");
        
        return userIDObj; 
    }


    [TestMethod]
    public async Task UnAuthorizedUser()
    {
        // Arrange
        var user = await GenerateRegUser().ConfigureAwait(false);
        
        // Convert user id from type obj to int
        int userID= Convert.ToInt32(user.data);
        
        EventManager.EventManager eventManager = new EventManager.EventManager();
        string title = "Beach Club";
        string description = "Beach clean up at Huntington Beach";
        var lat = 33.6603000;
        var lng = -117.9992300; 
        var expected = "User is not authorized. Event Creation is Inaccessible to Non-Reputable Users";
        
        // Act
        var result = await eventManager.CreateNewEvent(title, description, userID, lat, lng);
        
        // Assert
        Assert.IsFalse(result.isSuccessful);
        Assert.AreEqual(expected, result.errorMessage);
        SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
        await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
    }
    
    
    [TestMethod]
     public async Task ValidEventInsert()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         
        // Convert user id from type obj to int
         int userID= Convert.ToInt32(user.data);
         
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         
         var expected = "Event Successfully Created";

         // Act
         var result = await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         
         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
     }

     [TestMethod]
     public async Task EventCapacity()
     {
         // Arrange 
         var user = await GenerateReputableUser().ConfigureAwait(false);
         // Convert user id from type obj to int
         int userID= Convert.ToInt32(user.data);
         
         var regUser = await GenerateRegUser().ConfigureAwait(false);
         
         // Convert user id from type obj to int
         int regUserID = Convert.ToInt32(regUser.data);
         
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300; 
         var create = await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBUpdater daoUpdate = new SqlDAO(connString._connectionStringFeatures);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         Console.WriteLine(eventID);
         await daoUpdate.UpdateEventCount(eventID, 100).ConfigureAwait(false);
         var expected = "Unable To Join Event. Attendance Limit Has Been Met";
         
         // Act
         var result = await eventManager.JoinNewEvent(eventID, regUserID).ConfigureAwait(false);

         // Assert
         Assert.IsFalse(result.isSuccessful);
         Assert.AreEqual(expected, result.errorMessage);
         
         // Clean database to prevent Over population
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         await daoDelete.DeleteUserProfile(regUserID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     }

     [TestMethod]
     public async Task ValidJoinEvent()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         var regUser = await GenerateRegUser().ConfigureAwait(false);
         int regUserID = Convert.ToInt32(regUser.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300; 
         var expected = "You have joined the event";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         
         // Act
         var result = await eventManager.JoinNewEvent(eventID, regUserID).ConfigureAwait(false);

         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         await daoDelete.DeleteUserProfile(regUserID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     }

     [TestMethod]
     public async Task CannotJoinOwnEvent()
     {

         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID = Convert.ToInt32(user.data);
         EventManager.EventManager eventManager = new EventManager.EventManager();
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Park";
         float lat = 90;
         float lng = 99;
         var expected = "Cannot join your own event. You are the host";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);

         // Act
         var result = await eventManager.JoinNewEvent(eventID, userID).ConfigureAwait(false);

         // Assert
         Assert.IsFalse(result.isSuccessful);
         Assert.AreEqual(expected, result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
         
     }

     [TestMethod]
     public async Task JoinEventWithin7Secs()
     {
         // Arrange
         var stopwatch = new Stopwatch();
         var expected = 7000;
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID = Convert.ToInt32(user.data);
         var regUser = await GenerateRegUser().ConfigureAwait(false);
         int regUserID = Convert.ToInt32(regUser.data);
         EventManager.EventManager eventManager = new EventManager.EventManager();
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300; 
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);

         // Act
         stopwatch.Start();
         var result = await eventManager.JoinNewEvent(eventID, regUserID).ConfigureAwait(false);
         stopwatch.Stop();
         var actual = stopwatch.ElapsedMilliseconds;
         
         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.IsTrue(actual <= expected);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         await daoDelete.DeleteUserProfile(regUserID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     }

     [TestMethod]
     public async Task ValidLeaveEvent()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID = Convert.ToInt32(user.data);
         var regUser = await GenerateRegUser().ConfigureAwait(false);
         int regUserID = Convert.ToInt32(regUser.data);
         EventManager.EventManager eventManager = new EventManager.EventManager();
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         var expected = "You have left the event";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         await eventManager.JoinNewEvent(eventID, regUserID).ConfigureAwait(false);

         // Act
         var result = await eventManager.UnjoinNewEvent(eventID, regUserID).ConfigureAwait(false);

         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected, result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false); 
         
     }
     
     [TestMethod]
     public async Task LeaveEventWithin7Secs()
     {
         // Arrange
         var stopwatch = new Stopwatch();
         var expected = 7000;
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID = Convert.ToInt32(user.data);
         var regUser = await GenerateRegUser().ConfigureAwait(false);
         int regUserID = Convert.ToInt32(regUser.data);
         EventManager.EventManager eventManager = new EventManager.EventManager();
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         await eventManager.JoinNewEvent(eventID, regUserID).ConfigureAwait(false);

         // Act
         stopwatch.Start();
         var result = await eventManager.UnjoinNewEvent(eventID, regUserID).ConfigureAwait(false);
         stopwatch.Stop();
         var actual = stopwatch.ElapsedMilliseconds;
         
         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.IsTrue(actual <= expected);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         await daoDelete.DeleteUserProfile(regUserID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     }

     [TestMethod]
     public async Task CannotModifyAnotherEventTitle()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         var user2 = await GenerateReputableUser().ConfigureAwait(false);
         int userID2 = Convert.ToInt32(user2.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         var expected = "Error. Cannot Modify another user's event pin";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         
         // Act
         var result = await eventManager.UpdateNewTitleEvent("New Title",eventID, userID2).ConfigureAwait(false);

         // Assert
         Assert.IsFalse(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         await daoDelete.DeleteUserProfile(userID2).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     }
     
     [TestMethod]
     public async Task ValidModifyEventDescription()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         var expected = "Event Description Successfully Updated";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         
         // Act
         var result = await eventManager.UpdateNewDescriptionEvent("New Description",eventID, userID).ConfigureAwait(false);

         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     } 
     
     [TestMethod]
     public async Task ValidModifyEventTitle()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         var expected = "Event Title Successfully Updated";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         
         // Act
         var result = await eventManager.UpdateNewTitleEvent("New Title",eventID, userID).ConfigureAwait(false);

         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     }

     [TestMethod]
     public async Task EventCreateWithin7days()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         string title2 = "Beach Club";
         string description2 = "Beach clean up at Huntington Beach";
         var lat2 = 35.6603000;
         var lng2 = -118.9992300;
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         var expected = "Error Creating Event. Last Event Created is Within 7 days";
         
         // Act
         var result = await eventManager.CreateNewEvent(title2, description2, userID, lat2, lng2).ConfigureAwait(false);
         
         // Assert
         Assert.IsFalse(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     }

     [TestMethod]
     public async Task ValidDisplayAttendance()
     {
         // Arrange
         var user = await GenerateReputableUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         
         // No one joined
         var expected = 0; 
         
         // Act
         var result = await eventManager.DisplayAttendance(eventID, userID).ConfigureAwait(false);
         
         // Convert obj into int
         var count = Convert.ToInt32(result.data);
         
         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected, count);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false); 
     }
     
     [TestMethod]
     public async Task AdminValidModifyEventTitle()
     {
         // Arrange
         var user = await GenerateAdminUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         var expected = "Event Title Successfully Updated";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         
         // Act
         var result = await eventManager.UpdateNewTitleEvent("New Title",eventID, userID).ConfigureAwait(false);

         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
     } 
     
     [TestMethod]
     public async Task AdminValidModifyEventDescription()
     {
         // Arrange
         var user = await GenerateAdminUser().ConfigureAwait(false);
         int userID= Convert.ToInt32(user.data);
         EventManager.EventManager eventManager = new EventManager.EventManager(); 
         string title = "Beach Club";
         string description = "Beach clean up at Huntington Beach";
         var lat = 33.6603000;
         var lng = -117.9992300;
         var expected = "Event Description Successfully Updated";
         await eventManager.CreateNewEvent(title, description, userID, lat, lng).ConfigureAwait(false);
         IDBSelecter daoSelect = new SqlDAO(connString._connectionStringFeatures);
         SqlDAO daoDelete = new SqlDAO(connString._connectionStringUsers);
         var eventIDObj = await daoSelect.SelectEventID(userID).ConfigureAwait(false);
         int eventID = Convert.ToInt32(eventIDObj.data);
         
         // Act
         var result = await eventManager.UpdateNewDescriptionEvent("New Description",eventID, userID).ConfigureAwait(false);

         // Assert
         Assert.IsTrue(result.isSuccessful);
         Assert.AreEqual(expected,result.errorMessage);
         await daoDelete.DeleteUserProfile(userID).ConfigureAwait(false);
         IDBDeleter daoDeleter = new SQLDeletionDAO(connString._connectionStringFeatures);
         await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
         
     }  
     
     
     
     
     
     
     
     
     
     
     
     
     
     
}
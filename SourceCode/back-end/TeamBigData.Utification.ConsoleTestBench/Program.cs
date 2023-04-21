﻿// See https://aka.ms/new-console-template for more information

using TeamBigData.Utification.EventManager;
using TeamBigData.Utification.EventService;
using TeamBigData.Utification.EventService.Abstractions;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;


var connectionString = new DBConnectionString();
var dao = new SqlDAO(connectionString._connectionStringFeatures);
/* User Hash conversion
 * Gets a userID when inputing an email
var userID = await dao.SelectUserID("testUser@yahoo.com").ConfigureAwait(false);
Console.WriteLine(userID.isSuccessful);
Console.WriteLine(userID.data);
var userHash = await dao.SelectUserHash((int)userID.data).ConfigureAwait(false);
Console.WriteLine(userHash.data.ToString());
*/
/*
var data = await dao.SelectUserEvents(3178).ConfigureAwait(false);
Console.WriteLine(data);
foreach (var d in data)
{
  Console.WriteLine("Title: " + d._title);
}
*/

var eventManager = new EventManager();
/*
foreach (var d in data)
{
    Console.WriteLine(d._description);
}
*/
// var data = await eventManager.DisplayAllEvents().ConfigureAwait(false);
// Console.WriteLine(data);

// Event Joining
var data = await eventManager.JoinNewEvent(257, 3260).ConfigureAwait(false);
Console.WriteLine(data.errorMessage);

// var data2 = await eventManager.JoinNewEvent(210, 3178);

// Console.WriteLine(data2.errorMessage);
/*
var data3 = await eventManager.JoinNewEvent(211, 3178);
var data4 = await eventManager.JoinNewEvent(212, 3178);
var data5 = await eventManager.JoinNewEvent(214, 3178);
*/

/* Unjoin an Event
 var data4 = await eventManager.UnjoinNewEvent(215, 3178);
 Console.WriteLine(data4.errorMessage);
 */
/* checkinf if a user is owner of a pin
var dataObj = await dao.SelectEventOwner(207);
var data = Convert.ToInt32(dataObj.data);
Console.WriteLine(data);
if (data == 3179)
{
    Console.WriteLine("Owner of pin");
}
*/

// Deleting an event
/*
var data2 = await eventManager.DeleteNewEvent(221, 3157).ConfigureAwait(false);
Console.WriteLine(data2.errorMessage);
*/

/*
var data = await dao.SelectEventDate(3179).ConfigureAwait(false);
Console.WriteLine(data.data);
*/
/*
var data = await eventManager.MarkEventComplete(222, 3179);
Console.WriteLine(data.errorMessage);
*/

Console.WriteLine("Hello, World!");
Console.WriteLine("title: ");
var title = Console.ReadLine();
Console.WriteLine("description: ");
var description = Console.ReadLine();
var result = await eventManager.CreateNewEvent(title, description,3154, 90, -170).ConfigureAwait(false);
Console.WriteLine(result.errorMessage);
// Console.WriteLine(result.ToString());
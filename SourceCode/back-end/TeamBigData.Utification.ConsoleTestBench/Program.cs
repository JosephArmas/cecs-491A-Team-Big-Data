// See https://aka.ms/new-console-template for more information

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
var data = await dao.SelectUserEvents(3178).ConfigureAwait(false);
Console.WriteLine(data);
foreach (var d in data)
{
  Console.WriteLine("Title: " + d._title);
}
var eventManager = new EventManager();
// Event Joining
// var data = await eventManager.JoinNewEvent(207, 3178).ConfigureAwait(false);

// Console.WriteLine(data.errorMessage);
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

Console.WriteLine("Hello, World!");
Console.WriteLine("title: ");
var title = Console.ReadLine();
Console.WriteLine("description: ");
var description = Console.ReadLine();
var result = await eventManager.CreateNewEvent(title, description,3179).ConfigureAwait(false);
Console.WriteLine(result.errorMessage);
// Console.WriteLine(result.ToString());
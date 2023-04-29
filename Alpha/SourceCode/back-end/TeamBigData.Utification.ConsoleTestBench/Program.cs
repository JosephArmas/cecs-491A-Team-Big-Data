// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Components.Web;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.EventManager;
using TeamBigData.Utification.EventService;
using TeamBigData.Utification.EventService.Abstractions;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;


var connectionString = new DBConnectionString();
var dao = new SqlDAO(connectionString.devSqlUsers);
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
// var data = await eventManager.JoinNewEvent(257, 3260).ConfigureAwait(false);
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
EventService eventService = new EventService();

// var result = await dao.SelectUserProfileRole(3398).ConfigureAwait(false);
// Console.WriteLine(result.data);

// var data = await eventService.ReadRole(3398).ConfigureAwait(false);
// Console.WriteLine(data.data);

/*
var data = await eventService.ReadJoinedEvents(3352).ConfigureAwait(false);
if (data.Count == 0)
{
    Console.WriteLine("Empty List");
}
else
{
    foreach (var d in data)
    {
        Console.WriteLine(d._eventID);
    }
}
*/
bool IsValidEventID(Response response)
{
    var result = Convert.ToInt32(response.data);
    if (result >= 200 && response.isSuccessful)
        return true;
    else
        return false;
}

// var data = await eventService.ReadRole(3369).ConfigureAwait(false);
// Console.WriteLine(data.data);
// Console.WriteLine(IsValidEventID(data));
/*
if (data.data == null && !data.isSuccessful)
{
    Console.WriteLine("Event does not exist");
}
else
{
    Console.WriteLine("EVent exists: " + data.data);
}
*/

// var result = await eventManager.DisplayAttendance(343, 3410).ConfigureAwait(false);
// Console.WriteLine(result.isSuccessful);
// Console.WriteLine(result.data);
// var result = await eventManager.DisplayAllEvents().ConfigureAwait(false);
/*
var result2 = await eventManager.JoinNewEvent(376, 2108);
Console.WriteLine(result2.isSuccessful);
*/

var result = await eventManager.DisplayAllEvents().ConfigureAwait(false);
foreach (var d in result)
{
    Console.WriteLine(d._title);
    Console.WriteLine(d._description);
    Console.WriteLine(d._eventID);
}


// var result = await eventService.ReadAttendance(342).ConfigureAwait(false);
// Console.WriteLine(result.data);

Console.WriteLine("Hello, World!");
Console.WriteLine("title: ");
var title = Console.ReadLine();
Console.WriteLine("description: ");
var description = Console.ReadLine();
// var result = await eventManager.CreateNewEvent(title, description,3154, 90, -170).ConfigureAwait(false);
// Console.WriteLine(result.errorMessage);
// Console.WriteLine(result.ToString());
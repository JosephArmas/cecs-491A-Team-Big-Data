using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.EventManager;
using TeamBigData.Utification.EventService;
using TeamBigData.Utification.EventService.Abstractions;
using TeamBigData.Utification.Models;

namespace Utification.EntryPoint.Controllers;

[BindProperties]
public class EventPin
{
    public string title { get; set; }
    public string description { get; set; }
    public int userID { get; set; }
    public int eventID { get; set; }
    public double lat { get; set; }
    public double lng { get; set; }
}

public class EventController : Controller
{
    private EventDTO _eventDto;
    [Route("event/health")]
    [HttpGet]
    public Task<IActionResult> HealthCheck()
    {
        var tcs = new TaskCompletionSource<IActionResult>();
        tcs.SetResult(Ok("Working"));
        return tcs.Task;
    }

    /*
    public async Task<IActionResult> Helper(Task<Response> result, string option)
    {
        try
        {
            
        }
        catch (Exception e)
        {
            return BadRequest("Error in request");
        }
    }
    */

    [Route("event/createEventPin")]
    [HttpPost]
    public async Task<IActionResult> CreateEventPin([FromBody]EventPin eventPin)
    {
        var esMan = new EventManager();
        // Last line of defense
        try
        {
            var result = await esMan.CreateNewEvent(eventPin.title, eventPin.description,eventPin.userID,eventPin.lat,eventPin.lng).ConfigureAwait(false);
            return Ok(result.errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }
    }

    [Route("event/count")]
    [HttpPost]
    public async Task<IActionResult> DisplayAttendance([FromBody]EventPin eventPin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.DisplayAttendance(eventPin.eventID, eventPin.userID).ConfigureAwait(false);
            return Ok(result.data);

        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }

    }

    [Route("event/join")]
    public async Task<IActionResult> JoinEvent([FromBody] EventPin eventPin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.JoinNewEvent(eventPin.eventID, eventPin.userID).ConfigureAwait(false);
            return Ok(result.errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }
        
    }
    
    [Route("event/unjoin")]
    public async Task<IActionResult> UnJoinEvent([FromBody] EventPin eventPin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.UnjoinNewEvent(eventPin.eventID, eventPin.userID).ConfigureAwait(false);
            return Ok(result.errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }
        
    }
    
    [Route("event/getEventPins")]
    [HttpGet]
    public async Task<IActionResult> GetEventPins([FromBody] EventPin eventPin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.DisplayAllEvents().ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }
        
    }

    [Route("event/modifyEventTitle")]
    [HttpPost]
    public async Task<IActionResult> ModifyEventTitle([FromBody] EventPin eventpin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.UpdateNewTitleEvent(eventpin.title, eventpin.eventID,eventpin.userID).ConfigureAwait(false);
            return Ok(result.errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }
        
    }

    [Route("event/modifyEventDescription")]
    [HttpPost]
    public async Task<IActionResult> ModifyEventDescription([FromBody] EventPin eventPin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.UpdateNewDescriptionEvent(eventPin.description, eventPin.eventID,eventPin.userID).ConfigureAwait(false);
            return Ok(result.errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        } 
        
    }

    [Route("event/deleteEvent")]
    [HttpPost]
    public async Task<IActionResult> DeleteEvent([FromBody] EventPin eventPin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.DeleteNewEvent(eventPin.eventID, eventPin.userID);
            return Ok(result.errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }
    }

    [Route("event/markEventComplete")]
    [HttpPost]
    public async Task<IActionResult> MarkEventComplete([FromBody] EventPin eventPin)
    {
        var esMan = new EventManager();
        try
        {
            var result = await esMan.MarkEventComplete(eventPin.eventID, eventPin.userID);
            return Ok(result.errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest("Error in request.");
        }
    }



















































































}
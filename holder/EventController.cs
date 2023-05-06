using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.EventsManager;

namespace Utification.EntryPoint.Controllers
{
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
    
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventManager _eventManager;

        /*
        public EventController(EventManager eventManager)
        {
            _eventManager = eventManager;
        }
        */
        
        [HttpGet("health")]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
        }
        
        [HttpPost("createEventPin")]
        public async Task<IActionResult> CreateEventPin([FromBody]EventPin eventPin)
        {
            // Last line of defense
            try
            {
                var result = await _eventManager.CreateNewEvent(eventPin.title, eventPin.description,eventPin.userID,eventPin.lat,eventPin.lng).ConfigureAwait(false);
                return Ok(result.errorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
        }

        [HttpPost("count")]
        public async Task<IActionResult> DisplayAttendance([FromBody]EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.DisplayAttendance(eventPin.eventID, eventPin.userID).ConfigureAwait(false);
                return Ok(result.data);

            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }

        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinEvent([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.JoinNewEvent(eventPin.eventID, eventPin.userID).ConfigureAwait(false);
                return Ok(result.errorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
            
        }
        
        [HttpPost("unjoin")]
        public async Task<IActionResult> UnJoinEvent([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.UnjoinNewEvent(eventPin.eventID, eventPin.userID).ConfigureAwait(false);
                return Ok(result.errorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
            
        }
        
        [HttpGet("getEventPins")]
        public async Task<IActionResult> GetEventPins()
        {
            try
            {
                var result = await _eventManager.DisplayAllEvents().ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
            
        }

        [HttpPost("title")]
        public async Task<IActionResult> ModifyEventTitle([FromBody] EventPin eventpin)
        {
            try
            {
                var result = await _eventManager.UpdateNewTitleEvent(eventpin.title, eventpin.eventID,eventpin.userID).ConfigureAwait(false);
                return Ok(result.errorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
            
        }

        
        [HttpPost("description")]
        public async Task<IActionResult> ModifyEventDescription([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.UpdateNewDescriptionEvent(eventPin.description, eventPin.eventID,eventPin.userID).ConfigureAwait(false);
                return Ok(result.errorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            } 
            
        }

        
        [HttpPost("deleteEvent")]
        public async Task<IActionResult> DeleteEvent([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.DeleteNewEvent(eventPin.eventID, eventPin.userID);
                return Ok(result.errorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
        }

        
        [HttpPost("markEventComplete")]
        public async Task<IActionResult> MarkEventComplete([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.MarkEventComplete(eventPin.eventID, eventPin.userID);
                return Ok(result.errorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
        }
        
        
        [HttpPost("userEvents")]
        public async Task<IActionResult> UserEventPins([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.UserJoinedEvents(eventPin.userID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
        }

        [HttpPost("createdEvents")]
        public async Task<IActionResult> CreatedEvents([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.UserCreatedEvents(eventPin.userID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
        } 






    }
    
}

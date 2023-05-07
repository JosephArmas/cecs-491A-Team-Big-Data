using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.EventsManager;
using TeamBigData.Utification.Models.ControllerModels;

namespace Utification.EntryPoint.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly EventManager _eventManager;


        public EventController(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        [HttpGet("health")]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
        }

        [HttpPost("createEventPin")]
        public async Task<IActionResult> CreateEventPin([FromBody] EventPin eventPin)
        {
            // Last line of defense
            try
            {
                var result = await _eventManager.CreateNewEvent(eventPin.Title, eventPin.Description, eventPin.UserID, eventPin.Lat, eventPin.Lng).ConfigureAwait(false);
                return Ok(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
        }

        [HttpPost("count")]
        public async Task<IActionResult> DisplayAttendance([FromBody] EventPin eventPin)
        {
            try
            {
                var result = await _eventManager.DisplayAttendance(eventPin.EventID, eventPin.UserID).ConfigureAwait(false);
                return Ok();//result.data);

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
                var result = await _eventManager.JoinNewEvent(eventPin.EventID, eventPin.UserID).ConfigureAwait(false);
                return Ok(result.ErrorMessage);
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
                var result = await _eventManager.UnjoinNewEvent(eventPin.EventID, eventPin.UserID).ConfigureAwait(false);
                return Ok(result.ErrorMessage);
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
                var result = await _eventManager.UpdateNewTitleEvent(eventpin.Title, eventpin.EventID, eventpin.UserID).ConfigureAwait(false);
                return Ok(result.ErrorMessage);
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
                var result = await _eventManager.UpdateNewDescriptionEvent(eventPin.Description, eventPin.EventID, eventPin.UserID).ConfigureAwait(false);
                return Ok(result.ErrorMessage);
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
                var result = await _eventManager.DeleteNewEvent(eventPin.EventID, eventPin.UserID);
                return Ok(result.ErrorMessage);
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
                var result = await _eventManager.MarkEventComplete(eventPin.EventID, eventPin.UserID);
                return Ok(result.ErrorMessage);
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
                var result = await _eventManager.UserJoinedEvents(eventPin.UserID);
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
                var result = await _eventManager.UserCreatedEvents(eventPin.UserID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Error in request.");
            }
        }





    }

}

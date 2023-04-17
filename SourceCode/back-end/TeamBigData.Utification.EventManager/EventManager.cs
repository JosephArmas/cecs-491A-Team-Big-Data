
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.EventManager
{
    public class EventManager 
    {
        /* TODO:
         * Check title
         * Check description
         * check if pin posted within 7 days
         */
        // private readonly EventService.EventService _eventService;

        /*
        public EventManager(EventService.EventService eventService)
        {
            _eventService = eventService;
        }
        */
        
        public Response CreateNewEvent(string title, string description)
        {
            Response response = new Response();
            // Checking to see title or description is null or empty
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                response.errorMessage = "Trouble Displaying title or description. Please Try again";
            }
            else
            {
                response.errorMessage = "Success";
            }
            return response;

        }
        
    }
    
}


using System.Text.RegularExpressions;
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
            else if (!IsValidTitle(title))
            {
                response.errorMessage = "Error in Title. Please try again.";
            }
            else if (!IsValidDescription(description))
            {
                response.errorMessage = "Error in Description. Please Try again.";
            }
            else
            {
                response.isSuccessful = true;
                response.errorMessage = "Success";
                    
            }
            return response;

        }

        public bool IsValidTitle(string title)
        {
            Regex titleAllowedCharacters = new Regex(@"^[a-zA-Z0-9\s.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ-]*$");
            if (titleAllowedCharacters.IsMatch(title) && title.Length >= 8 && title.Length <= 30)
                return true;
            else
                return false;
        }

        public bool IsValidDescription(string description)
        {
            Regex descriptionAllowedCharacters = new Regex(@"^[a-zA-Z0-9\s.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ-]*$");
            if (descriptionAllowedCharacters.IsMatch(description) && description.Length <= 150)
                return true;
            else
            {
                return false;
            }

        }
        
    }
    
}

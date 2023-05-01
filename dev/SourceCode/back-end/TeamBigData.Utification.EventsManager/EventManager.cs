using System.Text.RegularExpressions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.EventsServices;

namespace TeamBigData.Utification.EventsManager
{
    
    public class EventManager
    {
        // Private
        private readonly EventService _eventService;
        
        private readonly string[] userRolesAuth =
        {
            "Reputable User", "Admin User"
        };

        private readonly string[] userRoles =
        {
            "Reputable User", "Service User", "Regular User"
        }; 
        
        // Public
        
        // Ctor w/ dependency injection
        public EventManager(EventService eventService)
        {
            _eventService = eventService;
        }
        
        
        //--------------------------
        // Check Statements
        //--------------------------
        
        // Check event ID
        public bool IsValidEventID(Response response)
        {
            var result = Convert.ToInt32(response.data);
            if (result>= 200 && response.isSuccessful)
                return true;
            else
                return false;
        }


        //  Check title
        public bool IsValidTitle(string title)
        {
            Regex titleAllowedCharacters = new Regex(@"^[a-zA-Z0-9\s.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ-]*$");
            if (titleAllowedCharacters.IsMatch(title) && title.Length >= 8 && title.Length <= 30)
                return true;
            else
                return false;
        }

        // Check title
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
        
        // Check if its within 7 days
        public bool IsValidCreateDate(DateTime date)
        {
            TimeSpan diff = DateTime.Now - date;
            if (diff.TotalDays > 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Using the bounds that Ghabe wrote in the front end
        // Checking lat and lng here again because front end can be insecure
        public bool IsValidPinBound(double lat, double lng)
        {
           if ((lat < 42.009517 && lat > 39) && (lng < -124 || lng > -120))
           {
               return false;
           }
           else if ((lat < 39 && lat > 38) && (lng < -124 || lng > -119))
           {
               return false;
           }
           else if ((lat < 38 && lat > 37) && (lng < -123 || lng > -118))
           {
               return false;
           }
           else if ((lat < 37 && lat > 36) && (lng < -122 || lng > -117))
           {
               return false;
           }
           else if ((lat < 36 && lat > 35) && (lng < -121 || lng > -116))
           {
               return false;
           }
           else if ((lat < 35 && lat > 34) && (lng < -120 || lng > -115))
           {
               return false;
           }
           else if ((lat < 34 && lat > 33) && (lng < -119 || lng > -114))
           {
               return false;
           }
           else if ((lat < 33 && lat > 32.528832) && (lng < -118 || lng > -114))
           {
               return false;
           }
           else
           {
               return true;
           } 
        } 
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
    
}

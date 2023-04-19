
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.EventService.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.EventManager
{
    public class EventManager
    {
        /* TODO:
         * Check title
         * Check description
         * check if pin posted within 7 days
         */
        
        private readonly DBConnectionString connString = new DBConnectionString();
        private readonly string[] userRolesAuth = {
            "Reputable User", "Admin User"
        };
        
        private readonly string[] userRoles= {
            "Reputable User", "Admin User", "Regular User"
        };
        
        
        // Check if a user is authorized
        /*
        private async Task<Response> IsAuthorized(int userId, string [] role)
        {
            IRead eServiceReader = new EventService.EventService();
            var result = await eServiceReader.ReadRole(userId).ConfigureAwait(false);
            return result;
        }
        */
        
        
        public async Task<Response> CreateNewEvent(string title, string description, int ownerID)
        {
            Response response = new Response();
            IRead eventServiceRead = new EventService.EventService();
            ICreate eventServiceCreate = new EventService.EventService();
            
            // Check if user has access to creating an event
            var role = await eventServiceRead.ReadRole(ownerID).ConfigureAwait(false);
            // var role = await IsAuthorized(ownerID, userRolesAuth).ConfigureAwait(false);
            if (!userRolesAuth.Contains(role.data))
            {
                response.errorMessage = "User is not authorized.";
            }
            else
            {
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
                    response = await eventServiceCreate.CreateEvent(title, description, ownerID).ConfigureAwait(false);
                    if (!response.isSuccessful)
                        response.errorMessage = "Error in inserting";
                    else
                    {
                        response.isSuccessful = true;
                    }

                }
            }
            return response;

        }

        public async Task<Response> JoinNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            ICreate joinEvent = new EventService.EventService();
            IRead eventReader = new EventService.EventService();
            var role = await eventReader.ReadRole(userID).ConfigureAwait(false);
            var countObj = await eventReader.ReadEventCount(eventID).ConfigureAwait(false);
            
            // Convert from obj to int (current count)
            int count = Convert.ToInt32(countObj.data);
            
            // Check for user's role before joining
            if (!userRoles.Contains(role.data))
            {
                response.errorMessage = "User is not authorized";
            }
                
            // Get Current count and check if its 0 - 100
            if (count >= 0 && count < 100)
            {
                response = await joinEvent.JoinEvent(eventID, userID).ConfigureAwait(false);
                if (response.isSuccessful)
                {
                    response.errorMessage = "You have joined the event";
                }
                else if (response.errorMessage.Contains("Violation of PRIMARY KEY"))
                {
                    response.errorMessage = "Event already joined";
                }
                else
                {
                    response.errorMessage = "Error Joining Event";
                }
            } 
            else if (count == 100)
            {
                response.errorMessage = "Unable to Join Event. Attendance Limit Has Been Met";
            } 
            else
            {
                response.errorMessage = "Error to Join Event. Please try again later.";
            }
            
            return response;
        }

        public async Task<Response> UnjoinNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            IRead eServiceReader = new EventService.EventService();
            IUpdate eServiceUpdater = new EventService.EventService();
            var countObj = await eServiceReader.ReadEventCount(eventID).ConfigureAwait(false);
            
            // Convert from obj to int (current count)
            int count = Convert.ToInt32(countObj.data);
            
            var role = await eServiceReader.ReadRole(userID).ConfigureAwait(false);
            if (!userRoles.Contains(role.data))
            {
                response.errorMessage = "User not Authorized";
            }
            
            // Validate the eventID passed is over 200
            if (eventID < 200)
            {
                response.errorMessage = "Error on retrieving event";
            }

            if (count == 0)
            {
                response.errorMessage = "Cannot Unjoin event. Attendance at 0";
            }
            else
            {
                response = await eServiceUpdater.UnjoinEvent(eventID, userID).ConfigureAwait(false);
                if (response.isSuccessful)
                {
                    response.errorMessage = "You have Unjoined Event.";
                }
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

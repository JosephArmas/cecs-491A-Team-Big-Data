
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        private readonly string[] userRolesAuth =
        {
            "Reputable User", "Admin User"
        };

        private readonly string[] userRoles =
        {
            "Reputable User", "Service User", "Regular User"
        };


        // Check if a user is authorized
        public async Task<bool> IsAuthorized(int userId, string[] role)
        {
            IRead esReader = new EventService.EventService();
            var result = await esReader.ReadRole(userId).ConfigureAwait(false);
            if (role.Contains(result.data))
                return true;
            else
            {
                return false;
            }

        }


        // As a Reputable User I can create an event
        public async Task<Response> CreateNewEvent(string title, string description, int ownerID, double lat, double lng)
        {
            Response response = new Response();
            IRead esRead = new EventService.EventService();
            ICreate esCreate = new EventService.EventService();
            var role = await esRead.ReadRole(ownerID).ConfigureAwait(false);

            // Check if User is authorized
            if (!await IsAuthorized(ownerID, userRolesAuth))
            {
                response.errorMessage = "User is not authorized. Event Creation is Inaccessible to Non-Reputable Users";
                return response;
            }

            if (role.data.ToString() == "Admin User")
            {
                response.errorMessage = "Error Admin cannot create events";
                return response;
            }
            else
            {
                var dateObj = await esRead.ReadEventDateCreated(ownerID).ConfigureAwait(false);

                // Convert date recieved from obj into DateTime
                DateTime dateCreated = Convert.ToDateTime(dateObj.data);
                

                // If there is nothing inserted inserted
                if (dateObj.data == null)
                {
                    // Checking to see title or description is null or empty (Business rules)
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
                    } else if (!IsValidPinBound(lat, lng))
                    {
                        response.errorMessage = "Pin Out of Bounds";
                    }
                    else
                    {
                        EventDTO eventDto = new EventDTO(title, description, ownerID, lat, lng);
                        response = await esCreate.CreateEvent(eventDto).ConfigureAwait(false);

                        // Check if it inserted into database
                        if (!response.isSuccessful)
                            response.errorMessage = "Error in inserting";
                        else
                        {
                            response.isSuccessful = true;
                            response.errorMessage = "Event Successfully Created";
                        }
                    }
                }
                else if (!IsValidCreateDate(dateCreated))
                {
                    response.errorMessage = "Error Creating Event. Last Event Created is Within 7 days";
                    return response;
                }
                else
                {
                    // Checking to see title or description is null or empty (Business rules)
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
                    } else if (!IsValidPinBound(lat, lng))
                    {
                        response.errorMessage = "Pin Out of Bounds";
                        
                    }
                    else
                    {
                        EventDTO eventDto = new EventDTO(title, description, ownerID, lat, lng);
                        response = await esCreate.CreateEvent(eventDto).ConfigureAwait(false);

                        // Check if it inserted into database
                        if (!response.isSuccessful)
                            response.errorMessage = "Error in inserting";
                        else
                        {
                            response.isSuccessful = true;
                            response.errorMessage = "Event Successfully Created";
                        }
                    }
                }

            }

            return response;
        }

        // As a User type Regular, Service or Reputable, I can join an event
        public async Task<Response> JoinNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            ICreate joinEvent = new EventService.EventService();
            IRead eventReader = new EventService.EventService();
            var countObj = await eventReader.ReadEventCount(eventID).ConfigureAwait(false);
            var ownerObj = await eventReader.ReadEventOwner(eventID).ConfigureAwait(false);
            Stopwatch stopwatch = new Stopwatch();
            // Convert obj to int to get User's ID
            int owner = Convert.ToInt32(ownerObj.data);

            // Convert from obj to int (current count)
            int count = Convert.ToInt32(countObj.data);

            stopwatch.Start();
            // Check for user's role before joining
            if (!await IsAuthorized(userID, userRoles))
            {
                response.errorMessage = "User is not authorized";
                return response;
            }

            // Check if eventID is valid
            var validEvent = await eventReader.ReadEvent(eventID).ConfigureAwait(false); 
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if its the event owner
            if (owner == userID)
            {
                response.errorMessage = "Cannot join your own event. You are the host";
                return response;
            }


            // Get Current count and check if its 0 - 100
            if (count >= 0 && count < 100)
            {
                response = await joinEvent.JoinEvent(eventID, userID).ConfigureAwait(false);
                stopwatch.Stop();
                if (response.isSuccessful)
                {
                    if (stopwatch.ElapsedMilliseconds <= 7000)
                    {
                        response.errorMessage = "You have joined the event";
                        return response;
                    }
                    else
                    {
                        response.errorMessage = "Process Time Out. Please Try Again";
                    }
                    
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
                response.errorMessage = "Unable To Join Event. Attendance Limit Has Been Met";
            }
            else
            {
                response.errorMessage = "Error to Join Event. Please try again later.";
            }

            return response;
        }

        // As a type Regular, Service or Reputable, I can Unjoin the event
        public async Task<Response> UnjoinNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            IRead esReader = new EventService.EventService();
            IUpdate eServiceUpdater = new EventService.EventService();
            var countObj = await esReader.ReadEventCount(eventID).ConfigureAwait(false);
            var ownerObj = await esReader.ReadEventOwner(eventID).ConfigureAwait(false);
            var roleObj = await esReader.ReadRole(userID);
            // Convert from obj to string
            var role = roleObj.data.ToString();
            Stopwatch stopwatch = new Stopwatch();

            // Convert obj to int to get User's ID
            int owner = Convert.ToInt32(ownerObj.data);

            // Convert from obj to int (current count)
            int count = Convert.ToInt32(countObj.data);

            stopwatch.Start();
            // Check if user is authorized
            if (!await IsAuthorized(userID, userRoles))
            {
                response.errorMessage = "User is not authorized";
                return response;
            }

            // Check if user is an admin
            if (role == "Admin User")
            {
                response.errorMessage = "Unable to unjoin as an Admin";
                return response;
            }
            
            // Check if user is owner
            if (owner == userID)
            {
                response.errorMessage = "Error. Cannot Unjoin. You are the host.";
                return response;
            }
            
            var joined = await esReader.ReadJoinedEvents(userID).ConfigureAwait(false);
            
            // Check if has joined any
            if (joined.Count == 0)
            {
                response.errorMessage = "Error. You have not joined an event";
                return response;
            }

            // Check if eventID is valid
            var validEvent = await esReader.ReadEvent(eventID).ConfigureAwait(false);
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Validate if count is 0
            if (count == 0)
            {
                response.errorMessage = "Cannot Unjoin event. Attendance at 0";
            }
            else
            {
                response = await eServiceUpdater.UnjoinEvent(eventID, userID).ConfigureAwait(false);
                if (response.isSuccessful)
                {
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds <= 7000)
                    {
                        response.errorMessage = "You have left the event";
                        response.isSuccessful = true;
                    }
                    else
                    {
                        response.errorMessage = "Process Timed Out. Please Try Again";
                    }
                }
                
            }


            return response;
        }

        public async Task<Response> DeleteNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            IRead esReader = new EventService.EventService();
            var role = await esReader.ReadRole(userID).ConfigureAwait(false);

            // Check if user is authorized
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.errorMessage = "User is not authorized";
            }

            // Check if eventID is valid
            var validEvent = await esReader.ReadEvent(eventID);
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await esReader.ReadEventOwner(eventID);

            // Convert obj into an int to compare
            int owner = Convert.ToInt32(ownerObj.data);

            if (owner != userID)
            {
                response.errorMessage = "Error. Cannot Delete another user's event pin.";
            }

            if (owner == userID || role.data.ToString() == "Admin User")
            {
                IDelete esDeleter = new EventService.EventService();
                response = await esDeleter.DeleteCreatedEvent(eventID, userID).ConfigureAwait(false);
                if (response.isSuccessful)
                {
                    response.errorMessage = "Event Successfully Deleted";
                }
                else
                {
                    response.errorMessage = "Something went wrong. Please try again.";
                }
            }

            return response;
        }

        // As an Event Owner, I can modify the title
        public async Task<Response> UpdateNewTitleEvent(string title, int eventID, int userID)
        {
            Response response = new Response();
            IRead esReader = new EventService.EventService();
            var role = await esReader.ReadRole(userID).ConfigureAwait(false);

            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.errorMessage = "User is not authorized";
                return response;
            }

            // Check if event ID is valid
            var validEvent = await esReader.ReadEvent(eventID).ConfigureAwait(false);
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Grabbing User's ID with corresponding event ID
            var ownerObj = await esReader.ReadEventOwner(eventID);

            // Convert obj into an int to compare user's ID
            int owner = Convert.ToInt32(ownerObj.data);

            // Check if user is the owner of the pin
            if (owner != userID)
            {
                response.errorMessage = "Error. Cannot Modify another user's event pin";
            }
            
            if (owner == userID || role.data.ToString() == "Admin User")
            {
                if (!IsValidTitle(title))
                {
                    response.errorMessage = "Error in Title. Please try again.";
                    return response;
                }

                IUpdate esUpdate = new EventService.EventService();
                response = await esUpdate.ModifyEventTitle(title, eventID).ConfigureAwait(false);
                if (response.isSuccessful)
                {
                    response.errorMessage = "Event Title Successfully Updated";
                }
                else
                {
                    response.errorMessage = "Error updating title. Please try again";
                }

            }

            return response;
        }

        // As an event owner I can modify the description
        public async Task<Response> UpdateNewDescriptionEvent(string description, int eventID, int userID)
        {
            Response response = new Response();
            IRead esReader = new EventService.EventService();
            var role = await esReader.ReadRole(userID).ConfigureAwait(false);

            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.errorMessage = "User is not authorized";

                return response;
            }

            // Check if event ID is valid
            var validEvent = await esReader.ReadEvent(eventID);
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await esReader.ReadEventOwner(eventID);

            // Convert obj into an int to compare user's ID
            int owner = Convert.ToInt32(ownerObj.data);

            if (owner != userID)
            {
                response.errorMessage = "Error. Cannot Modify another user's event pin.";
            }

            if (owner == userID || role.data.ToString() == "Admin User")
            {
                if (!IsValidDescription(description))
                {
                    response.errorMessage = "Error in Title. Please try again.";
                }

                IUpdate esUpdate = new EventService.EventService();
                response = await esUpdate.ModifyEventDescription(description, eventID).ConfigureAwait(false);
                if (response.isSuccessful)
                {
                    response.errorMessage = "Event Description Successfully Updated";
                }
                else
                {
                    response.errorMessage = "Error updating description. Please try again";
                }

            }

            return response;
        }

        // As an owner, I can mark the event as completed
        public async Task<Response> MarkEventComplete(int eventID, int userID)
        {
            Response response = new Response();
            IRead esReader = new EventService.EventService();
            // User Role stored in type obj
            var role = await esReader.ReadRole(userID).ConfigureAwait(false);


            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.errorMessage = "User is not authorized";

                return response;
            }

            // Check if event ID is valid
            var validEvent = await esReader.ReadEvent(eventID);
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }
            

            // Check if user is the owner of the pin
            var ownerObj = await esReader.ReadEventOwner(eventID);

            // Convert obj into an int to compare user's ID
            int owner = Convert.ToInt32(ownerObj.data);

            if (owner != userID)
            {
                response.errorMessage = "Error. Cannot Modify another user's event pin.";
            }

            // Check if event pin is the owner or is type Admin
            if (owner == userID || role.data.ToString() == "Admin User")
            {
                IUpdate esUpdate = new EventService.EventService();
                var result = await esUpdate.ModifyEventDisabled(eventID).ConfigureAwait(false);
                if (result.isSuccessful)
                {
                    response.isSuccessful = true;
                    response.errorMessage = "Event Pin Marked as Completed";
                    response.data = result.data;
                    return response;
                }
                else
                {
                    response.errorMessage = "Error in completeing pin. Please Try again";
                }
            }


            return response;
        }

        public async Task<List<EventDTO>> DisplayAllEvents()
        {
            IRead esReader = new EventService.EventService();
            return await esReader.ReadAllEvents();
        }

        // As an owner, I can display the attendance
        public async Task<Response> DisplayAttendance(int eventID, int userID)
        {
            Response response = new Response();
            IRead esReader = new EventService.EventService();
            IUpdate esUpdate = new EventService.EventService();
            
            // User Role stored in type obj
            var role = await esReader.ReadRole(userID).ConfigureAwait(false);


            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.errorMessage = "User is not authorized";

                return response;
            }

            // Check if event ID is valid
            var validEvent = await esReader.ReadEvent(eventID).ConfigureAwait(false);
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await esReader.ReadEventOwner(eventID);

            // Convert obj into an int to compare user's ID
            int owner = Convert.ToInt32(ownerObj.data);

            if (owner != userID)
            {
                response.errorMessage = "Error. Cannot Modify another user's event pin.";
                return response;
            }
            else
            {
                var validDisplay = await esReader.ReadAttendance(eventID).ConfigureAwait(false);
                
                // Convert from obj to int
                var attendanceFlag = Convert.ToInt32(validDisplay.data);

                if (validDisplay.isSuccessful && attendanceFlag == 1)
                {
                    return await esReader.ReadEventCount(eventID).ConfigureAwait(false);
                }

                // Check if the flag is already set
                if (validDisplay.isSuccessful && attendanceFlag == 0)
                {
                    var result = await esUpdate.ModifyEventAttendance(eventID).ConfigureAwait(false);
                    if (result.isSuccessful)
                    {
                         return await esReader.ReadEventCount(eventID).ConfigureAwait(false);
                        // response.isSuccessful = true;
                        // return response;
                    }
                    else
                    {
                        response.errorMessage = "Error updating";
                    }
                    
                }
                
            }

            return response;

        }
        
        public async Task<Response> DisableAttendance(int eventID, int userID)
        {
            Response response = new Response();
            IRead esReader = new EventService.EventService();
            IUpdate esUpdate = new EventService.EventService();
            // User Role stored in type obj
            var role = await esReader.ReadRole(userID).ConfigureAwait(false);


            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.errorMessage = "User is not authorized";

                return response;
            }

            // Check if event ID is valid
            var validEvent = await esReader.ReadEvent(eventID).ConfigureAwait(false);
            if (!IsValidEventID(validEvent))
            {
                response.errorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await esReader.ReadEventOwner(eventID);

            // Convert obj into an int to compare user's ID
            int owner = Convert.ToInt32(ownerObj.data);

            if (owner != userID)
            {
                response.errorMessage = "Error. Cannot Modify another user's event pin.";
                return response;
            }
            else
            {
                var validDisplay = await esReader.ReadAttendance(eventID).ConfigureAwait(false);
                
                
                // Convert from obj to int
                var attendanceFlag = Convert.ToInt32(validDisplay.data);

                // Check if its already set
                if (validDisplay.isSuccessful && attendanceFlag == 1)
                {
                    var result = await esUpdate.ModifyEventAttendanceDisable(eventID).ConfigureAwait(false);
                    if (result.isSuccessful)
                    {
                        response.isSuccessful = true;
                        response.errorMessage = "Disabled Attendance";

                    }
                    else
                    {
                        response.errorMessage = "Error updating";
                    }
                        
                }
                
                if (attendanceFlag == 0)
                {
                    response.errorMessage = "Attendance already disabled";
                    return response;
                }
                
            }

            return response;

        }
        


        // Check event id
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
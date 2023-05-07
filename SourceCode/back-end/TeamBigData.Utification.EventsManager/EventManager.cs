using System.Diagnostics;
using System.Text.RegularExpressions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.EventsServices;
using TeamBigData.Utification.Models;
//using TeamBigData.Utification.SQLDataAccess.UsersDB;

namespace TeamBigData.Utification.EventsManager
{

    public class EventManager
    {
        // Private
        private readonly EventService _eventService;
        // private readonly UsersSqlDAO _usersSql;

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
            // _usersSql = usersSql;
        }


        //--------------------------
        // Check Statements
        //--------------------------


        // TODO: Use InputValidation class from ErrorResponse
        //  Check title
        public bool IsValidTitle(string title)
        {
            Regex titleAllowedCharacters = new Regex(@"^[a-zA-Z0-9\s.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ-]*$");
            if (titleAllowedCharacters.IsMatch(title) && title.Length >= 8 && title.Length <= 30)
                return true;
            else
                return false;
        }

        // Check description
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


        // Check if a user is authorized
        // TODO: result should turn into DataResponse afther chanigng readrole
        public async Task<bool> IsAuthorized(int userId, string[] role)
        {
            var result = await _eventService.ReadRole(userId).ConfigureAwait(false);
            if (role.Contains(result.Data))
                return true;
            else
            {
                return false;
            }
            
        }


        // As a Reputable User I can create an event
        // TODO: Have read role return dataresponse with the proper response
        // Recommend to start doing the negation of your checks to early exit like hes been explaining to take out the nesting
        public async Task<Response> CreateNewEvent(string title, string description, int ownerID, double lat, double lng)
        {
            Response response = new Response();
            var role = await _eventService.ReadRole(ownerID).ConfigureAwait(false);

            // Check if User is authorized
            if (!await IsAuthorized(ownerID, userRolesAuth))
            {
                response.ErrorMessage = "User is not authorized. Event Creation is Inaccessible to Non-Reputable Users";
                return response;
            }

            if (role.Data.ToString() == "Admin User")
            {
                response.ErrorMessage = "Error Admin cannot create events";
                return response;
            }
            else
            {
                var dateObj = await _eventService.ReadEventDateCreated(ownerID).ConfigureAwait(false);

                // Convert date recieved from obj into DateTime
                DateTime dateCreated = Convert.ToDateTime(dateObj.Data);


                // If there is nothing inserted inserted
                if (dateObj.Data == null)
                {
                    // Checking to see title or description is null or empty (Business rules)
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
                    {
                        response.ErrorMessage = "Trouble Displaying title or description. Please Try again";
                    }
                    else if (!InputValidation.IsValidTitle(title))
                    {
                        response.ErrorMessage = "Error in Title. Please try again.";
                    }
                    else if (!InputValidation.IsValidDescription(description))
                    {
                        response.ErrorMessage = "Error in Description. Please Try again.";
                    }
                    else if (!InputValidation.IsValidPinBound(lat, lng))
                    {
                        response.ErrorMessage = "Pin Out of Bounds";
                    }
                    else
                    {
                        EventDTO eventDto = new EventDTO(title, description, ownerID, lat, lng);
                        response = await _eventService.CreateEvent(eventDto).ConfigureAwait(false);

                        // Check if it inserted into database
                        if (!response.IsSuccessful)
                            response.ErrorMessage = "Error in inserting";
                        else
                        {
                            response.IsSuccessful = true;
                            response.ErrorMessage = "Event Successfully Created";
                        }
                    }
                }
                else if (!InputValidation.IsValidCreateDate(dateCreated))
                {
                    response.ErrorMessage = "Error Creating Event. Last Event Created is Within 7 days";
                    return response;
                }
                else
                {
                    // Checking to see title or description is null or empty (Business rules)
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
                    {
                        response.ErrorMessage = "Trouble Displaying title or description. Please Try again";
                    }
                    else if (!IsValidTitle(title))
                    {
                        response.ErrorMessage = "Error in Title. Please try again.";
                    }
                    else if (!IsValidDescription(description))
                    {
                        response.ErrorMessage = "Error in Description. Please Try again.";
                    }
                    else if (!InputValidation.IsValidPinBound(lat, lng))
                    {
                        response.ErrorMessage = "Pin Out of Bounds";

                    }
                    else
                    {
                        EventDTO eventDto = new EventDTO(title, description, ownerID, lat, lng);
                        response = await _eventService.CreateEvent(eventDto).ConfigureAwait(false);

                        // Check if it inserted into database
                        if (!response.IsSuccessful)
                            response.ErrorMessage = "Error in inserting";
                        else
                        {
                            response.IsSuccessful = true;
                            response.ErrorMessage = "Event Successfully Created";
                        }
                    }
                }

            }

            return response;
        }

        // As a User type Regular, Service or Reputable, I can join an event
        // TODO: Have owners return dataresponse with the proper response
        public async Task<Response> JoinNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            var countObj = await _eventService.ReadEventCount(eventID).ConfigureAwait(false);
            var ownerObj = await _eventService.ReadEventOwner(eventID).ConfigureAwait(false);
            Stopwatch stopwatch = new Stopwatch();
            int owner = ownerObj.Data;
            int count = countObj.Data;

            stopwatch.Start();
            // Check for user's role before joining
            if (!await IsAuthorized(userID, userRoles))
            {
                response.ErrorMessage = "User is not authorized";
                return response;
            }

            // Check if eventID is valid
            var validEvent = await _eventService.ReadEvent(eventID).ConfigureAwait(false);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if its the event owner
            if (owner == userID)
            {
                response.ErrorMessage = "Cannot join your own event. You are the host";
                return response;
            }

            // Get Current count and check if its 0 - 100
            if (count >= 0 && count < 100)
            {
                response = await _eventService.JoinEvent(eventID, userID).ConfigureAwait(false);
                stopwatch.Stop();
                if (response.IsSuccessful)
                {
                    if (stopwatch.ElapsedMilliseconds <= 7000)
                    {
                        response.ErrorMessage = "You have joined the event";
                        return response;
                    }
                    else
                    {
                        response.ErrorMessage = "Process Time Out. Please Try Again";
                    }

                }
                else if (response.ErrorMessage.Contains("Violation of PRIMARY KEY"))
                {
                    response.ErrorMessage = "Event already joined";
                }
                else
                {
                    response.ErrorMessage = "Error Joining Event";
                }
            }
            else if (count == 100)
            {
                response.ErrorMessage = "Unable To Join Event. Attendance Limit Has Been Met";
            }
            else
            {
                response.ErrorMessage = "Error to Join Event. Please try again later.";
            }

            return response;
        }

        // As a type Regular, Service or Reputable, I can Unjoin the event
        public async Task<Response> UnjoinNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            Stopwatch stopwatch = new Stopwatch();
            var countObj = await _eventService.ReadEventCount(eventID).ConfigureAwait(false);
            var ownerObj = await _eventService.ReadEventOwner(eventID).ConfigureAwait(false);
            var roleObj = await _eventService.ReadRole(userID);
            
            // Storing role, owner and count and start Stopwatch
            var role = roleObj.Data;
            int owner = ownerObj.Data;
            int count = countObj.Data;
            stopwatch.Start();
            
            // Check if user is authorized
            if (!await IsAuthorized(userID, userRoles))
            {
                response.ErrorMessage = "User is not authorized";
                return response;
            }

            // Check if user is an admin
            if (role == "Admin User")
            {
                response.ErrorMessage = "Unable to unjoin as an Admin";
                return response;
            }

            // Check if user is owner
            if (owner == userID)
            {
                response.ErrorMessage = "Error. Cannot Unjoin. You are the host.";
                return response;
            }

            var joined = await _eventService.ReadJoinedEvents(userID).ConfigureAwait(false);

            // Check if has joined any
            if (joined.Count == 0)
            {
                response.ErrorMessage = "Error. You have not joined an event";
                return response;
            }

            // Check if eventID is valid
            var validEvent = await _eventService.ReadEvent(eventID).ConfigureAwait(false);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Validate if count is 0
            if (count == 0)
            {
                response.ErrorMessage = "Cannot Unjoin event. Attendance at 0";
            }
            else
            {
                response = await _eventService.UnjoinEvent(eventID, userID).ConfigureAwait(false);
                if (response.IsSuccessful)
                {
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds <= 7000)
                    {
                        response.ErrorMessage = "You have left the event";
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.ErrorMessage = "Process Timed Out. Please Try Again";
                    }
                }

            }


            return response;
        }

        public async Task<Response> DeleteNewEvent(int eventID, int userID)
        {
            Response response = new Response();
            var role = await _eventService.ReadRole(userID).ConfigureAwait(false);

            // Check if user is authorized
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.ErrorMessage = "User is not authorized";
            }

            // Check if eventID is valid
            var validEvent = await _eventService.ReadEvent(eventID);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await _eventService.ReadEventOwner(eventID);
            int owner = ownerObj.Data;

            if (owner != userID)
            {
                response.ErrorMessage = "Error. Cannot Delete another user's event pin.";
            }

            if (owner == userID || role.Data == "Admin User")
            {
                response = await _eventService.DeleteCreatedEvent(eventID, userID).ConfigureAwait(false);
                if (response.IsSuccessful)
                {
                    response.ErrorMessage = "Event Successfully Deleted";
                }
                else
                {
                    response.ErrorMessage = "Something went wrong. Please try again.";
                }
            }

            return response;
        }

        // As an Event Owner, I can modify the title
        public async Task<Response> UpdateNewTitleEvent(string title, int eventID, int userID)
        {
            Response response = new Response();
            var role = await _eventService.ReadRole(userID).ConfigureAwait(false);

            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.ErrorMessage = "User is not authorized";
                return response;
            }

            // Check if event ID is valid
            var validEvent = await _eventService.ReadEvent(eventID).ConfigureAwait(false);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Grabbing User's ID with corresponding event ID
            var ownerObj = await _eventService.ReadEventOwner(eventID);
            int owner = ownerObj.Data;

            // Check if user is the owner of the pin
            if (owner != userID)
            {
                response.ErrorMessage = "Error. Cannot Modify another user's event pin";
            }

            if (owner == userID || role.Data== "Admin User")
            {
                if (!IsValidTitle(title))
                {
                    response.ErrorMessage = "Error in Title. Please try again.";
                    return response;
                }

                response = await _eventService.ModifyEventTitle(title, eventID).ConfigureAwait(false);
                if (response.IsSuccessful)
                {
                    response.ErrorMessage = "Event Title Successfully Updated";
                }
                else
                {
                    response.ErrorMessage = "Error updating title. Please try again";
                }

            }

            return response;
        }

        // As an event owner I can modify the description
        public async Task<Response> UpdateNewDescriptionEvent(string description, int eventID, int userID)
        {
            Response response = new Response();
            var role = await _eventService.ReadRole(userID).ConfigureAwait(false);

            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.ErrorMessage = "User is not authorized";

                return response;
            }

            // Check if event ID is valid
            var validEvent = await _eventService.ReadEvent(eventID);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await _eventService.ReadEventOwner(eventID);
            int owner = ownerObj.Data;

            if (owner != userID)
            {
                response.ErrorMessage = "Error. Cannot Modify another user's event pin.";
            }

            if (owner == userID || role.Data == "Admin User")
            {
                if (!IsValidDescription(description))
                {
                    response.ErrorMessage = "Error in Title. Please try again.";
                }

                response = await _eventService.ModifyEventDescription(description, eventID).ConfigureAwait(false);
                if (response.IsSuccessful)
                {
                    response.ErrorMessage = "Event Description Successfully Updated";
                }
                else
                {
                    response.ErrorMessage = "Error updating description. Please try again";
                }

            }

            return response;
        }

        // As an owner, I can mark the event as completed
        public async Task<Response> MarkEventComplete(int eventID, int userID)
        {
            Response response = new Response();
            // User Role stored in type obj
            var role = await _eventService.ReadRole(userID).ConfigureAwait(false);

            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.ErrorMessage = "User is not authorized";
                
                return response;
            }

            // Check if event ID is valid
            var validEvent = await _eventService.ReadEvent(eventID);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                
                return response;
            }


            // Check if user is the owner of the pin
            var ownerObj = await _eventService.ReadEventOwner(eventID);

            // Convert obj into an int to compare user's ID
            int owner = ownerObj.Data;

            if (owner != userID)
            {
                response.ErrorMessage = "Error. Cannot Modify another user's event pin.";
            }

            // Check if event pin is the owner or is type Admin
            if (owner == userID || role.Data == "Admin User")
            {
                var result = await _eventService.ModifyEventDisabled(eventID).ConfigureAwait(false);
                if (result.IsSuccessful)
                {
                    response.IsSuccessful = true;
                    response.ErrorMessage = "Event Pin Marked as Completed";
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error in completeing pin. Please Try again";
                }
            }


            return response;
        }

        public async Task<List<EventDTO>> DisplayAllEvents()
        {
            return await _eventService.ReadAllEvents();
        }

        // As an owner, I can display the attendance
        public async Task<DataResponse<int>> DisplayAttendance(int eventID, int userID)
        {
            var response = new DataResponse<int>();

            // User Role stored in type obj
            var role = await _eventService.ReadRole(userID).ConfigureAwait(false);


            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.ErrorMessage = "User is not authorized";

                return response;
            }

            // Check if event ID is valid
            var validEvent = await _eventService.ReadEvent(eventID).ConfigureAwait(false);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await _eventService.ReadEventOwner(eventID);
            int owner = ownerObj.Data;

            if (owner != userID)
            {
                response.ErrorMessage = "Error. Cannot Modify another user's event pin.";
                return response;
            }
            else
            {
                var validDisplay = await _eventService.ReadAttendance(eventID).ConfigureAwait(false);

                // Convert from obj to int
                var attendanceFlag = validDisplay.Data;

                if (validDisplay.IsSuccessful && attendanceFlag == 1)
                {
                    return await _eventService.ReadEventCount(eventID).ConfigureAwait(false);
                }

                // Check if the flag is already set
                if (validDisplay.IsSuccessful && attendanceFlag == 0)
                {
                    var result = await _eventService.ModifyEventAttendance(eventID).ConfigureAwait(false);
                    if (result.IsSuccessful)
                    {
                        return await _eventService.ReadEventCount(eventID).ConfigureAwait(false);
                        // response.isSuccessful = true;
                        // return response;
                    }
                    else
                    {
                        response.ErrorMessage = "Error updating";
                    }

                }

            }

            return response;
        }

        public async Task<Response> DisableAttendance(int eventID, int userID)
        {
            Response response = new Response();
            // User Role stored in type obj
            var role = await _eventService.ReadRole(userID).ConfigureAwait(false);


            // Is the user Authorized?
            if (!await IsAuthorized(userID, userRolesAuth))
            {
                response.ErrorMessage = "User is not authorized";

                return response;
            }

            // Check if event ID is valid
            var validEvent = await _eventService.ReadEvent(eventID).ConfigureAwait(false);
            if (!InputValidation.IsValidEventID(validEvent))
            {
                response.ErrorMessage = "Error with event or Event does not exist.";
                return response;
            }

            // Check if user is the owner of the pin
            var ownerObj = await _eventService.ReadEventOwner(eventID);
            int owner = ownerObj.Data;

            if (owner != userID)
            {
                response.ErrorMessage = "Error. Cannot Modify another user's event pin.";
                return response;
            }
            else
            {
                var validDisplay = await _eventService.ReadAttendance(eventID).ConfigureAwait(false);


                // Convert from obj to int
                var attendanceFlag = validDisplay.Data;

                // Check if its already set
                if (validDisplay.IsSuccessful && attendanceFlag == 1)
                {
                    var result = await _eventService.ModifyEventAttendanceDisable(eventID).ConfigureAwait(false);
                    if (result.IsSuccessful)
                    {
                        response.IsSuccessful = true;
                        response.ErrorMessage = "Disabled Attendance";

                    }
                    else
                    {
                        response.ErrorMessage = "Error updating";
                    }

                }

                if (attendanceFlag == 0)
                {
                    response.ErrorMessage = "Attendance already disabled";
                    return response;
                }

            }

            return response;
        }

        public async Task<List<EventDTO>> UserJoinedEvents(int userID)
        {
            return await _eventService.ReadJoinedEvents(userID);
        }

        public async Task<List<EventDTO>> UserCreatedEvents(int userID)
        {
            return await _eventService.ReadCreatedEvents(userID);
        }

    }

}

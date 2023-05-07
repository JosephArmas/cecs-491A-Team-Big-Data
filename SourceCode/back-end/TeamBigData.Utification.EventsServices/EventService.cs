using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.EventsServices
{
    public class EventService
    {
        private readonly IEventDBSelect _iEventDbSelect;
        private readonly IEventDBInsert _iEventDbInsert;
        private readonly IEventDBUpdate _iEventDbUpdate;
        private readonly IEventDBDelete _iEventDbDelete;
        private readonly IUsersDBSelecter _dbSelecter;
        private readonly ILogger _logger;

        // Ctor w/ dependency injection
        public EventService(EventsSqlDAO sqlDao, UsersSqlDAO usersSqlDao, ILogger logger)
        {
            _iEventDbSelect = sqlDao;
            _iEventDbInsert = sqlDao;
            _iEventDbUpdate = sqlDao;
            _iEventDbDelete = sqlDao;
            _dbSelecter = usersSqlDao;
            _logger = logger;
        }


        //------------------------
        // Read
        //------------------------

        // Read Role
        public async Task<DataResponse<string>> ReadRole(int userID)
        {
            return await _dbSelecter.SelectUserProfileRole(userID).ConfigureAwait(false);
        }

        // Read Event Count
        public async Task<DataResponse<int>> ReadEventCount(int eventID)
        {

            return await _iEventDbSelect.SelectEventCount(eventID).ConfigureAwait(false);
        }

        // Read Event Owner
        public async Task<DataResponse<int>> ReadEventOwner(int eventID)
        {
            return await _iEventDbSelect.SelectEventOwner(eventID);
        }

        // Read Joined Events
        public async Task<List<EventDTO>> ReadJoinedEvents(int userID)
        {
            List<EventDTO> events = await _iEventDbSelect.SelectJoinedEvents(userID).ConfigureAwait(false);

            return events;
        }

        // Read Event Date Created
        public async Task<DataResponse<DateTime>> ReadEventDateCreated(int userID)
        {

            return await _iEventDbSelect.SelectEventDate(userID).ConfigureAwait(false);
        }

        // Read all events
        public async Task<List<EventDTO>> ReadAllEvents()
        {
            List<EventDTO> events = await _iEventDbSelect.SelectAllEvents().ConfigureAwait(false);
            return events;
        }

        // Read Event
        public async Task<DataResponse<int>> ReadEvent(int userID)
        {
            return await _iEventDbSelect.SelectEventPin(userID).ConfigureAwait(false);
        }

        // Read Attendance
        public async Task<DataResponse<int>> ReadAttendance(int eventID)
        {
            return await _iEventDbSelect.SelectAttendance(eventID).ConfigureAwait(false);
        }

        // Read Created Events
        public async Task<List<EventDTO>> ReadCreatedEvents(int userID)
        {
            List<EventDTO> events = await _iEventDbSelect.SelectUserEvents(userID).ConfigureAwait(false);
            return events;

        }


        //-------------------
        // Insert
        //-------------------
        // TODO: Maybe keep logging at the manager layer
        public async Task<Response> CreateEvent(EventDTO eventDto)
        {

            var hashObj = await _dbSelecter.SelectUserHash(eventDto.UserID).ConfigureAwait(false);
            // Convert from obj to string
            string userHash = hashObj.Data.ToString();
            var result = await _iEventDbInsert.InsertEvent(eventDto).ConfigureAwait(false);
            if (result.IsSuccessful)
            {
                await _logger.Logs(new Log(1, "Info", userHash, "EventService.CreateEvent()", "Data", "Event Created Successful"));
            }

            return result;

        }

        //---------------------------------
        // Update
        //---------------------------------

        // Join Event
        public async Task<Response> JoinEvent(int eventID, int userID)
        {

            var result = await _iEventDbInsert.InsertJoinEvent(eventID, userID);
            if (result.IsSuccessful)
            {
                await _iEventDbUpdate.IncrementEvent(eventID).ConfigureAwait(false);
            }

            return result;
        }

        // Unjoin Event
        public async Task<Response> UnjoinEvent(int eventID, int userID)
        {
            var result = await _iEventDbDelete.DeleteJoinedEvent(userID, eventID).ConfigureAwait(false);

            // Sql Successfully operates on table
            if (result.IsSuccessful)
            {
                // Decrement the count
                await _iEventDbUpdate.DecrementEvent(eventID).ConfigureAwait(false);
            }
            else
            {
                result.ErrorMessage = "Error in Unjoining event. Please try again";
            }

            return result;
        }

        // Modify Event Title
        public async Task<Response> ModifyEventTitle(string title, int eventID)
        {
            return await _iEventDbUpdate.UpdateEventTitle(title, eventID).ConfigureAwait(false);

        }

        // Modify Event Description
        public async Task<Response> ModifyEventDescription(string description, int eventID)
        {

            return await _iEventDbUpdate.UpdateEventDescription(description, eventID);
        }

        // Modify Event to Disabled  (completed)
        public async Task<Response> ModifyEventDisabled(int eventID)
        {

            return await _iEventDbUpdate.UpdateEventToDisabled(eventID).ConfigureAwait(false);
        }

        // Modify Event Attendance
        public async Task<Response> ModifyEventAttendance(int eventID)
        {

            return await _iEventDbUpdate.UpdateEventAttendanceShow(eventID);
        }

        // Modify Event Attendance to disabled
        public async Task<Response> ModifyEventAttendanceDisable(int eventID)
        {

            return await _iEventDbUpdate.UpdateEventAttendanceDisable(eventID);
        }

        //--------------------------
        // Delete
        //--------------------------
        public async Task<Response> DeleteCreatedEvent(int eventID, int userID)
        {

            return await _iEventDbDelete.DeleteEvent(eventID).ConfigureAwait(false);
        }

    }

}

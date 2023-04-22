using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.EventService.Abstractions;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.EventService
{
    public class EventService: ICreate, IRead, IUpdate, IDelete 
    {
        // Private
        private readonly DBConnectionString connString;
        
        
        
        


        /*
        private async Task<Response> ReadUserHash(int userID)
        {
            var dao = new SqlDAO(connString._connectionStringUsers);
            return await dao.SelectUserHash(userID);

        }
        */

        
        // Public
        public async Task<Response> ReadRole(int userID)
        {
            IDBSelecter dao = new SqlDAO(connString.devSqlUsers);
            return await dao.SelectUserProfileRole(userID);
        }

        public async Task<Response> ReadEventCount(int eventID)
        {
            Response response= new Response();
            IDBSelecter dao = new SqlDAO(connString.devSqlFeatures);
            return await dao.SelectEventCount(eventID).ConfigureAwait(false);
        }

        public async Task<Response> CreateEvent(EventDTO eventDto)
        {
            
            IDBInserter daoInserter = new SqlDAO(connString.devSqlFeatures);
            IDBSelecter hashDao = new SqlDAO(connString.devSqlUsers);
            var logDao = new SqlDAO(connString.devSqlLogs);
            var hashObj = await hashDao.SelectUserHash(eventDto._userID).ConfigureAwait(false);
            // Convert from obj to string
            string userHash = hashObj.data.ToString();
            Log log;
            ILogger logger = new Logger(logDao);
            var result = await daoInserter.InsertEvent(eventDto).ConfigureAwait(false);
            if (result.isSuccessful)
            {
                log = new Log(1, "Info", userHash,"EventService.CreateEvent()", "Data", "Event Created Successful");
                logger.Log(log);
            }
            
            return result;

        }
        
        public async Task<Response> JoinEvent(int eventID, int userID)
        {
            
            IDBUpdater daoUpdate = new SqlDAO(connString.devSqlFeatures);
            IDBInserter daoInsert = new SqlDAO(connString.devSqlFeatures);
            var result = await daoInsert.InsertJoinEvent(eventID, userID);
            if (result.isSuccessful) 
            {
                await daoUpdate.IncrementEvent(eventID).ConfigureAwait(false);
            }
            
            return result;
        }

        public async Task<Response> UnjoinEvent(int eventID, int userID)
        {
            IDBUpdater daoUpdate = new SqlDAO(connString.devSqlFeatures);
            IDBDeleter daoDelete = new SQLDeletionDAO(connString.devSqlFeatures);
            var result = await daoDelete.DeleteJoinedEvent(userID, eventID).ConfigureAwait(false);
            
            // Sql Successfully operates on table
            if (result.isSuccessful)
            {
                // Decrement the count
                await daoUpdate.DecrementEvent(eventID).ConfigureAwait(false);
            }
            else
            {
                result.errorMessage = "Error in Unjoining event. Please try again";
            }

            return result;
        }

        public async Task<Response> ReadEventOwner(int eventID)
        {
            IDBSelecter daoSelecter = new SqlDAO(connString.devSqlFeatures);

            return await daoSelecter.SelectEventOwner(eventID);
            
        }

        public async Task<List<EventDTO>> ReadJoinedEvents(int userID)
        {
            IDBSelecter daoSelect = new SqlDAO(connString.devSqlFeatures);

            return await daoSelect.SelectJoinedEvents(userID).ConfigureAwait(false);
        }

        public async Task<Response> DeleteCreatedEvent(int eventID,int userID)
        {
            IDBDeleter daoDeleter = new SQLDeletionDAO(connString.devSqlFeatures);

            return await daoDeleter.DeleteEvent(eventID).ConfigureAwait(false);
        }

        public async Task<Response> ModifyEventTitle(string title, int eventID)
        {
            IDBUpdater daoUpdate = new SqlDAO(connString.devSqlFeatures);
            return await daoUpdate.UpdateEventTitle(title, eventID).ConfigureAwait(false);
            
        }

        public async Task<Response> ModifyEventDescription(string description, int eventID)
        {
            IDBUpdater daoUpdate = new SqlDAO(connString.devSqlFeatures);

            return await daoUpdate.UpdateEventDescription(description, eventID);
        }

        public async Task<Response> ReadEventDateCreated(int userID)
        {
            IDBSelecter daoSelect = new SqlDAO(connString.devSqlFeatures);
            
             return await daoSelect.SelectEventDate(userID).ConfigureAwait(false);
        }

        public async Task<Response> ModifyEventDisabled(int eventID)
        {
            IDBUpdater daoUpdate = new SqlDAO(connString.devSqlFeatures);

            return await daoUpdate.UpdateEventToDisabled(eventID).ConfigureAwait(false);
        }

        public async Task<List<EventDTO>> ReadAllEvents()
        {
            IDBSelecter daoSelect = new SqlDAO(connString.devSqlFeatures);
            List<EventDTO> events = await daoSelect.SelectAllEvents().ConfigureAwait(false);
            return events;
        }

        public async Task<Response> ReadEvent(int eventID)
        {
            IDBSelecter daoSelect = new SqlDAO(connString.devSqlFeatures);
            return await daoSelect.SelectEventPin(eventID).ConfigureAwait(false);
        }

    }
    
}

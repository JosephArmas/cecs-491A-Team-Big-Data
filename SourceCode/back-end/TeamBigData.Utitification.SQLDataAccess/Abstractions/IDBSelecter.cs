using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBSelecter
    {
        public Task<DataResponse<UserProfile>> SelectUserProfile(int userID);
        public Task<DataResponse<List<UserProfile>>> SelectUserProfileTable(String role);
        public Task<DataResponse<UserAccount>> SelectUserAccount(String username);
        public Task<DataResponse<List<UserAccount>>> SelectUserAccountTable(String role);
        public Task<Response> SelectLastUserID();
        public Task<DataResponse<List<Pin>>> SelectPinTable();
        public Task<Response> SelectUserProfileRole(int userID);
        public Task<Response> SelectUserHash(int userID);
        public Task<Response> SelectUserID(string email);
        public Task<Response> SelectEventCount(int eventID);
        public Task<Response> SelectEventOwner(int eventID);
        public Task<Response> SelectEventDate(int userID);
        public Task<List<EventDTO>> SelectAllEvents();
        public Task<List<EventDTO>> SelectUserEvents(int userID);
        public Task<List<EventDTO>> SelectJoinedEvents(int userID);
        public Task<Response> SelectEventPin(int eventID);
        
        public Task<Response> SelectEventID(int userID);

    }
}

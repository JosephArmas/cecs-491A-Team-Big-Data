using System.Net.NetworkInformation;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBUpdater
    {
        public Task<Response> UpdateUserProfile(UserProfile user);
        public Task<Response> UpdatePinToComplete(int pinID);
        public Task<Response> UpdatePinType(int pinID, int pinType);
        public Task<Response> UpdatePinContent(int pinID, string description);
        public Task<Response> UpdatePinToDisabled(int pinID);
        public Task<Response> IncrementEvent(int eventID);
        public Task<Response> DecrementEvent(int eventID);
        public Task<Response> UpdateEventTitle(string title, int eventID);
        public Task<Response> UpdateEventDescription(string description, int eventID);
        public Task<Response> UpdateEventToDisabled(int eventID);
        public Task<Response> UpdateUserRole(int userID, string role);
        public Task<Response> UpdateEventCount(int eventID, int count);
        public Task<Response> UpdateEventAttendanceShow(int eventID);
        public Task<Response> UpdateEventAttendanceDisable(int eventID);

    }
}

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
    }
}

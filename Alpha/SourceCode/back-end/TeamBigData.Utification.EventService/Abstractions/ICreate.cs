using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.EventService.Abstractions
{
    public interface ICreate
    {
        // public Task<Response> CreateEvent(string title, string description,int ownerID, float lat, float lng); 
        public Task<Response> CreateEvent(EventDTO eventDto); 
        public Task<Response> JoinEvent(int eventID, int userID);

    }
    
    
}

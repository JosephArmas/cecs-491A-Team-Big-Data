using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.EventService.Abstractions
{
    public interface ICreate
    {
        public Task<Response> CreateEvent(string title, string description,int ownerID); 
        public Task<Response> JoinEvent(int eventID, int userID);

    }
    
    
}

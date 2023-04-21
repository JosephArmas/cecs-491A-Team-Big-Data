using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.EventService.Abstractions
{
    public interface IRead
    {
        public Task<Response> ReadRole(int userID);
        public Task<Response> ReadEventCount(int eventID);
        public Task<Response> ReadEventOwner(int eventID);
        public Task<Response> ReadEventDateCreated(int userID);
        public Task<List<EventDTO>> ReadAllEvents();

    }
    
}

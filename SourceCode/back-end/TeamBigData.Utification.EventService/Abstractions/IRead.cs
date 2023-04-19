using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.EventService.Abstractions
{
    public interface IRead
    {
        public Task<Response> ReadRole(int userID);
        public Task<Response> ReadEventCount(int eventID);

    }
    
}

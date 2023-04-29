using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.EventService.Abstractions
{
    public interface IDelete
    {
        public Task<Response> DeleteCreatedEvent(int eventID, int userID);
    }
}

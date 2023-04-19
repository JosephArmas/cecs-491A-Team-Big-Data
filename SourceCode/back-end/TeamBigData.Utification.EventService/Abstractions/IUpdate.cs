using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.EventService.Abstractions;

public interface IUpdate
{
    public Task<Response> UnjoinEvent(int eventID, int userID);

}
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.EventService.Abstractions;

public interface IUpdate
{
    public Task<Response> UnjoinEvent(int eventID, int userID);
    public Task<Response> ModifyEventTitle(string title, int eventID);
    public Task<Response> ModifyEventDescription(string description, int eventID);
    public Task<Response> ModifyEventDisabled(int eventID);

}
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;

public interface IEventDBDelete
{
    public Task<Response> DeleteEvent(int eventID);
    public Task<Response> DeleteJoinedEvent(int eventID, int userID);

}
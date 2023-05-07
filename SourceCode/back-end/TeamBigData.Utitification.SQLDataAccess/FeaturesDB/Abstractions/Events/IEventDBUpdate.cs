using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;

public interface IEventDBUpdate
{
    public Task<Response> UpdateUserRole(int userID, string role);
    public Task<Response> IncrementEvent(int eventID);
    public Task<Response> DecrementEvent(int eventID);
    public Task<Response> UpdateEventCount(int eventID, int count);
    public Task<Response> UpdateEventAttendanceShow(int eventID);
    public Task<Response> UpdateEventAttendanceDisable(int eventID);
    public Task<Response> UpdateEventToDisabled(int eventID);
    public Task<Response> UpdateEventTitle(string title, int eventID);
    public Task<Response> UpdateEventDescription(string description, int eventID);
}
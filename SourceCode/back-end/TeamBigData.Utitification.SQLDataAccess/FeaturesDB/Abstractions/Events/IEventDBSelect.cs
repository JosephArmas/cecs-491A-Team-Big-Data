using Microsoft.EntityFrameworkCore.Diagnostics;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;

public interface IEventDBSelect
{
    public Task<List<EventDTO>> SelectUserCreatedEvents(int userID);
    public Task<List<EventDTO>> SelectAllEvents();
    public Task<DataResponse<int>> SelectEventPin(int eventID);
    public Task<List<EventDTO>> SelectUserEvents(int userID);
    public Task<List<EventDTO>> SelectJoinedEvents(int userID);
    public Task<DataResponse<int>> SelectEventID(int userID);
    public Task<DataResponse<int>> SelectEventCount(int eventID);
    public Task<DataResponse<int>> SelectEventOwner(int eventID);
    public Task<DataResponse<int>> SelectAttendance(int eventID);
    public Task<DataResponse<DateTime>> SelectEventDate(int userID);

}
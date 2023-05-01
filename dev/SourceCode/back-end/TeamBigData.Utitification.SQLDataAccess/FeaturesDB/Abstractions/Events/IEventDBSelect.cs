using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;

public interface IEventDBSelect
{
    public Task<Response> SelectUserProfileRole(int userID);
    public Task<Response> SelectUserID(string email);
    public Task<Response> SelectUserHash(int userID);
    public Task<List<EventDTO>> SelectUserCreatedEvents(int userID);
    public Task<List<EventDTO>> SelectAllEvents();
    public Task<Response> SelectEventPin(int eventID);
    public Task<List<EventDTO>> SelectUserEvents(int userID);
    public Task<List<EventDTO>> SelectJoinedEvents(int userID);
    public Task<Response> SelectEventID(int userID);
    public Task<Response> SelectEventCount(int eventID);
    public Task<Response> SelectEventOwner(int eventID);
    public Task<Response> SelectAttendance(int eventID);
    public Task<Response> SelectEventDate(int userID);



















}
using Microsoft.EntityFrameworkCore.Diagnostics;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;

public interface IEventDBInsert
{
    public Task<Response> InsertEvent(EventDTO eventDto);
    public Task<Response> InsertJoinEvent(int eventID, int userID);
}
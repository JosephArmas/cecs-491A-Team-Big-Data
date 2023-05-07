using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB;

public class EventsSqlDAO : DbContext, IEventDBInsert, IEventDBSelect, IEventDBUpdate, IEventDBDelete
{

    // Private
    private readonly String _connectionString;
    private async Task<Response> ExecuteSqlCommand(SqlConnection connection, SqlCommand command)
    {
        var tcs = new TaskCompletionSource<Response>();
        Response result = new Response();
        //Executes the SQL Insert Statement using the Connection String provided
        try
        {
            connection.Open();
            var rows = command.ExecuteNonQuery();
            if (rows > 0)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "SqlCommand Passed";
            }
            else if (rows == 0)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Nothing Affected";
            }
            connection.Close();
        }
        catch (SqlException s)
        {
            result.ErrorMessage = s.Message + ", {failed: command.ExecuteNonQuery}";
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.Message + ", {failed: command.ExecuteNonQuery}";
        }
        tcs.SetResult(result);
        return result;
    }

    // Public 
    public EventsSqlDAO(DbContextOptions<EventsSqlDAO> options) : base(options)
    {
        _connectionString = this.Database.GetDbConnection().ConnectionString;
    }

    public EventsSqlDAO(string connectionString)
    {
        _connectionString = connectionString;
    }


    //--------------------------
    // Insert
    //--------------------------
    // Insert Event
    public Task<Response> InsertEvent(EventDTO eventDto)
    {
        var currentDate = DateTime.Now.Date;
        var connection = new SqlConnection(_connectionString);
        var insertSql = "INSERT INTO dbo.Events (title, description,eventCreated, userID, lat, lng) values(@title,@description,@eventCreated,@userID,@lat,@lng)";
        var cmd = new SqlCommand(insertSql, connection);
        cmd.Parameters.AddWithValue("@title", eventDto.Title);
        cmd.Parameters.AddWithValue("@description", eventDto.Description);
        cmd.Parameters.AddWithValue("@eventCreated", currentDate);
        cmd.Parameters.AddWithValue("@userID", eventDto.UserID);
        cmd.Parameters.AddWithValue("@lat", eventDto.Lat);
        cmd.Parameters.AddWithValue("@lng", eventDto.Lng);
        return ExecuteSqlCommand(connection, cmd);
    }

    // Insert Joined Event
    public Task<Response> InsertJoinEvent(int eventID, int userID)
    {
        var connection = new SqlConnection(_connectionString);
        var insertSql = "INSERT INTO dbo.EventsJoined (userID, eventID) values(@userID,@eventID)";
        var cmd = new SqlCommand(insertSql, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        cmd.Parameters.AddWithValue("@userID", userID);
        return ExecuteSqlCommand(connection, cmd);
    }






    //--------------------------
    // Select
    //--------------------------

    // Events
    // Select User Created Events
    public async Task<List<EventDTO>> SelectUserCreatedEvents(int userID)
    {
        var sqlstatement = "SELECT title, description FROM Events WHERE userID = @userID AND disabled != 1";
        List<EventDTO> events = new List<EventDTO>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@userID", userID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string title = reader.GetString(reader.GetOrdinal("title"));
                    string description = reader.GetString(reader.GetOrdinal("description"));
                    events.Add(new EventDTO(title, description));
                }

            }

        }

        return events;

    }

    // Select All Events
    public async Task<List<EventDTO>> SelectAllEvents()
    {
        var sqlstatement = "SELECT title, description, eventID , lat, lng, count FROM Events WHERE disabled != 1";
        List<EventDTO> events = new List<EventDTO>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string title = reader.GetString(reader.GetOrdinal("title"));
                    string description = reader.GetString(reader.GetOrdinal("description"));
                    int eventID = reader.GetInt32(reader.GetOrdinal("eventID"));
                    double lat = reader.GetDouble(reader.GetOrdinal("lat"));
                    double lng = reader.GetDouble(reader.GetOrdinal("lng"));
                    int count = reader.GetInt32((reader.GetOrdinal("count")));
                    events.Add(new EventDTO(title, description, lat, lng, eventID, count));
                }

            }
        }

        return events;
    }

    // Select Event Pin
    // TODO: Change SelectEventPin to return DataResponse with the proper datatype for the response
    public async Task<DataResponse<int>> SelectEventPin(int eventID)
    {
        var sqlstatement = "SELECT eventID FROM dbo.Events WHERE eventID = @eventID";
        var response = new DataResponse<int>();
        using (var connection = new SqlConnection(_connectionString))
        {
            // Open the connection async
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);

            cmd.Parameters.AddWithValue("@eventID", eventID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int eventPin = reader.GetInt32(reader.GetOrdinal("eventID"));

                        response.Data = eventPin;
                        response.IsSuccessful = true;

                    }
                }
                else
                {
                    response.ErrorMessage = "Error getting event ID";

                }
            }
        }

        return response;
    }

    // Select User's Events
    // TODO: Put the list as the template for a DataResponse ex. public async Task<DataResponse<List<EventDTO>>> SelectUserEvents(int userID)
    public async Task<List<EventDTO>> SelectUserEvents(int userID)
    {
        var sqlstatement = "SELECT title, description , eventID FROM Events WHERE userID = @userID";
        List<EventDTO> events = new List<EventDTO>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@userID", userID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string title = reader.GetString(reader.GetOrdinal("title"));
                    string description = reader.GetString(reader.GetOrdinal("description"));
                    int eventID = reader.GetInt32(reader.GetOrdinal("eventID"));
                    events.Add(new EventDTO(title, description, eventID));
                }

            }

        }

        return events;
    }

    // Select Joined Events
    // TODO:  Put the list as the template for a DataResponse
    public async Task<List<EventDTO>> SelectJoinedEvents(int userID)
    {
        var sqlstatement = "SELECT title, description , Events.eventID FROM Events JOIN EventsJoined EJ on Events.eventID = EJ.eventID WHERE EJ.userID = @userID";
        List<EventDTO> events = new List<EventDTO>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@userID", userID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string title = reader.GetString(reader.GetOrdinal("title"));
                    string description = reader.GetString(reader.GetOrdinal("description"));
                    int eventID = reader.GetInt32(reader.GetOrdinal("eventID"));
                    events.Add(new EventDTO(title, description, eventID));
                }

            }

        }
        return events;
    }

    // Select Event ID
    // TODO: Change SelectEventID to return DataResponse with the proper datatype for the response
    public async Task<DataResponse<int>> SelectEventID(int userID)
    {
        var sqlstatement = "SELECT eventID FROM dbo.Events WHERE userID = @userID";
        var response = new DataResponse<int>();
        using (var connection = new SqlConnection(_connectionString))
        {
            // Open the connection async
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);

            cmd.Parameters.AddWithValue("@userID", userID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int eventID = reader.GetInt32(reader.GetOrdinal("eventID"));

                        response.Data = eventID;
                        response.IsSuccessful = true;

                    }
                }
                else
                {
                    response.ErrorMessage = "Error getting event ID";

                }
            }
        }

        return response;
    }

    // Select Event Count
    // TODO: Change SelectEventCount to return DataResponse with the proper datatype for the response
    public async Task<DataResponse<int>> SelectEventCount(int eventID)
    {
        var sqlstatement = "SELECT count FROM dbo.Events WHERE eventID = @eventID";
        var response = new DataResponse<int>();
        using (var connection = new SqlConnection(_connectionString))
        {
            // Open the connection async
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);

            // Sql to return the userHash associated with the passed in userID 
            cmd.Parameters.AddWithValue("@eventID", eventID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int count = reader.GetInt32(reader.GetOrdinal("count"));

                        response.Data = count;
                        response.IsSuccessful = true;

                    }
                }
                else
                {
                    response.ErrorMessage = "Error getting event count";

                }
            }
        }

        return response;
    }

    // Select Event Owner
    // TODO: Change SelectEventOwner to return DataResponse with the proper datatype for the response
    public async Task<DataResponse<int>> SelectEventOwner(int eventID)
    {
        var sqlstatement = "SELECT userID FROM dbo.Events WHERE eventID = @eventID";
        var response = new DataResponse<int>();
        using (var connection = new SqlConnection(_connectionString))
        {
            // Open the connection async
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);
            // Sql to return the userHash associated with the passed in userID 
            cmd.Parameters.AddWithValue("@eventID", eventID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int owner = reader.GetInt32(reader.GetOrdinal("userID"));

                        // Response obj stores the userHash value inside of the data property
                        response.Data = owner;
                        response.IsSuccessful = true;

                    }
                }
                else
                {
                    response.ErrorMessage = "Error getting event count";

                }
            }
        }

        return response;
    }

    // Select Attendance
    // TODO: Change SelectEventCount to return DataResponse with the proper datatype for the response
    public async Task<DataResponse<int>> SelectAttendance(int eventID)
    {
        var sqlstatement = "SELECT showAttendance FROM dbo.Events WHERE eventID = @eventID";
        var response = new DataResponse<int>();
        using (var connection = new SqlConnection(_connectionString))
        {
            // Open the connection async
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);

            // Sql to return the userHash associated with the passed in userID 
            cmd.Parameters.AddWithValue("@eventID", eventID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Date stored in type obj
                        response.Data = reader.GetInt32(reader.GetOrdinal("showAttendance"));
                        response.IsSuccessful = true;
                    }
                }
                else
                {
                    response.ErrorMessage = "Error Getting Event Date Created";

                }
            }
        }

        return response;
    }

    // Select Event Date
    // TODO: Change SelectEventDate to return DataResponse with the proper datatype for the response
    public async Task<DataResponse<DateTime>> SelectEventDate(int userID)
    {
        var sqlstatement = "SELECT eventCreated FROM dbo.Events WHERE userID = @userID";
        var response = new DataResponse<DateTime>();
        using (var connection = new SqlConnection(_connectionString))
        {
            // Open the connection async
            await connection.OpenAsync();
            var cmd = new SqlCommand(sqlstatement, connection);

            // Sql to return the userHash associated with the passed in userID 
            cmd.Parameters.AddWithValue("@userID", userID);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Date stored in type obj
                        response.Data = reader.GetDateTime(reader.GetOrdinal("eventCreated"));
                        response.IsSuccessful = true;
                    }
                }
                else
                {
                    response.ErrorMessage = "Error Getting Event Date Created";

                }
            }
        }

        return response;
    }





    //-------------------------
    // Update
    //-------------------------
    // Authorization
    public Task<Response> UpdateUserRole(int userID, string role)
    {
        var sqlstatement = "UPDATE dbo.UserProfiles SET role = @r WHERE userID = @userID";
        var response = new Response();
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        // Sql to return the userHash associated with the passed in userID 
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@r", role);
        return ExecuteSqlCommand(connection, cmd);

    }

    // Increment Event
    public Task<Response> IncrementEvent(int eventID)
    {
        // Sql statement to increment the value
        var sqlstatement = "UPDATE dbo.Events SET count = count + 1 WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        return ExecuteSqlCommand(connection, cmd);
    }

    // Decrement Event
    public Task<Response> DecrementEvent(int eventID)
    {
        // Sql statement to increment the value
        var sqlstatement = "UPDATE dbo.Events SET count = count - 1 WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        return ExecuteSqlCommand(connection, cmd);
    }

    // Update Event Count
    public Task<Response> UpdateEventCount(int eventID, int count)
    {
        // Sql statement to increment the value
        var sqlstatement = "UPDATE dbo.Events SET count = @count WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        cmd.Parameters.AddWithValue("@count", count);
        return ExecuteSqlCommand(connection, cmd);
    }

    // Update Event to show
    public Task<Response> UpdateEventAttendanceShow(int eventID)
    {
        // Sql statement to increment the value
        var sqlstatement = "UPDATE dbo.Events SET showAttendance = 1 WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        return ExecuteSqlCommand(connection, cmd);
    }

    // Update Event to disable
    public Task<Response> UpdateEventAttendanceDisable(int eventID)
    {
        // Sql statement to increment the value
        var sqlstatement = "UPDATE dbo.Events SET showAttendance = 0 WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);

        return ExecuteSqlCommand(connection, cmd);
    }

    // Update Event Title
    public Task<Response> UpdateEventTitle(string title, int eventID)
    {
        var sqlstatement = "UPDATE dbo.Events SET title = @title WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        cmd.Parameters.AddWithValue("@title", title);
        return ExecuteSqlCommand(connection, cmd);
    }

    // Update Event Description
    public Task<Response> UpdateEventDescription(string description, int eventID)
    {
        var sqlstatement = "UPDATE dbo.Events SET description = @description WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        cmd.Parameters.AddWithValue("@description", description);
        return ExecuteSqlCommand(connection, cmd);
    }

    // Update Event to Disabled
    public Task<Response> UpdateEventToDisabled(int eventID)
    {
        var sqlstatement = "UPDATE dbo.Events SET disabled = 1 WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);

        return ExecuteSqlCommand(connection, cmd);

    }



    //---------------------------------- 
    // Delete
    //---------------------------------- 

    // Delete Event
    public Task<Response> DeleteEvent(int eventID)
    {
        var sqlstatement = "DELETE FROM dbo.Events WHERE eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);

        return ExecuteSqlCommand(connection, cmd);
    }

    // Delete Joined Event
    public Task<Response> DeleteJoinedEvent(int userID, int eventID)
    {
        var sqlstatement = "DELETE FROM dbo.EventsJoined WHERE userID = @userID and eventID = @eventID";
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(sqlstatement, connection);
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@eventID", eventID);

        return ExecuteSqlCommand(connection, cmd);

    }




}

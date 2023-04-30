using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Events;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB;

public class EventsSqlDAO: DbContext, IEventDBInsert, IEventDBSelect, IEventDBUpdate, IEventDBDelete
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
                result.isSuccessful = true;
                result.errorMessage = "SqlCommand Passed";
            }
            else if (rows == 0)
            {
                result.isSuccessful = true;
                result.errorMessage = "Nothing Affected";
            }
            connection.Close();
        }
        catch (SqlException s)
        {
            result.errorMessage = s.Message + ", {failed: command.ExecuteNonQuery}";
        }
        catch (Exception e)
        {
            result.errorMessage = e.Message + ", {failed: command.ExecuteNonQuery}";
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
        cmd.Parameters.AddWithValue("@title", eventDto._title);
        cmd.Parameters.AddWithValue("@description", eventDto._description);
        cmd.Parameters.AddWithValue("@eventCreated",currentDate);
        cmd.Parameters.AddWithValue("@userID", eventDto._userID);
        cmd.Parameters.AddWithValue("@lat", eventDto._lat);
        cmd.Parameters.AddWithValue("@lng", eventDto._lng);
        return ExecuteSqlCommand(connection,cmd);
    }
    
    // Insert Joined Event
    public Task<Response> InsertJoinEvent(int eventID, int userID)
    {
        var connection = new SqlConnection(_connectionString);
        var insertSql = "INSERT INTO dbo.EventsJoined (userID, eventID) values(@userID,@eventID)";
        var cmd = new SqlCommand(insertSql, connection);
        cmd.Parameters.AddWithValue("@eventID", eventID);
        cmd.Parameters.AddWithValue("@userID", userID);
        return ExecuteSqlCommand(connection,cmd);
    } 
    
    
    
    
    
    
    //--------------------------
    // Select
    //--------------------------
    // Authorization User ID
   public async Task<Response> SelectUserID(string email)
        {
            var sqlstatement = "SELECT userID FROM dbo.Users WHERE username = @email";
            var response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                // Open the connection async
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                // Sql to return the userHash associated with the passed in userID 
                cmd.Parameters.AddWithValue("@email", email);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            int userID = reader.GetInt32(reader.GetOrdinal("userID"));
                            
                            // Response obj stores the userHash value inside of the data property
                            response.data = userID;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting User Hash";

                    }
                }
            }

            return response;

        }
        
        // Authorization User Role
        public async Task<Response> SelectUserProfileRole(int userID)
        {
            string sqlStatement = "SELECT role FROM dbo.UserProfiles WHERE userID = @userID";
            // var connection = new SqlConnection(_connectionString);
            var response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlStatement,connection);
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string role = reader.GetString(reader.GetOrdinal("role"));
                            response.data = role;
                            response.isSuccessful = true;
                        }
                    }
                    else
                    {
                        response.errorMessage = "No role associated with user ID";

                    }
                }
            }


            return response;
        } 
        
        // Authorization User Hash
        public async Task<Response> SelectUserHash(int userID)
        {
            var sqlstatement = "SELECT userHash FROM dbo.Users WHERE userID = @userID";
            var response = new Response();
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
                            
                            string userHash = reader.GetString(reader.GetOrdinal("userHash"));
                            
                            // Response obj stores the userHash value inside of the data property
                            response.data = userHash;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting User Hash";

                    }
                }
            }

            return response;
        } 
        
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
            var sqlstatement = "SELECT title, description, eventID , lat, lng FROM Events WHERE disabled != 1";
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
                        double lat= reader.GetDouble(reader.GetOrdinal("lat"));
                        double lng = reader.GetDouble(reader.GetOrdinal("lng"));
                        events.Add(new EventDTO(title, description, lat, lng, eventID));
                    }

                }
            }
            
            return events;
        } 
        
        // Select Event Pin
        public async Task<Response> SelectEventPin(int eventID)
        {
            var sqlstatement = "SELECT eventID FROM dbo.Events WHERE eventID = @eventID";
            var response = new Response();
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
                            
                            response.data = eventPin;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event ID";

                    }
                }
            }
            
            return response; 
        }
        
        // Select User's Events
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
                        events.Add(new EventDTO(title,description, eventID));
                    }
                    
                }

            }

            return events;
        } 
        
        // Select Joined Events
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
                        events.Add(new EventDTO(title,description, eventID));
                    }
                    
                }

            }
            return events;
        }

        // Select Event ID
        public async Task<Response> SelectEventID(int userID)
        {
            var sqlstatement = "SELECT eventID FROM dbo.Events WHERE userID = @userID";
            var response = new Response();
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
                            
                            response.data = eventID;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event ID";

                    }
                }
            }
            
            return response;  
        }
 
        // Select Event Count
        public async Task<Response> SelectEventCount(int eventID)
        {
            var sqlstatement = "SELECT count FROM dbo.Events WHERE eventID = @eventID";
            var response = new Response();
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
                            
                            response.data = count;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event count";

                    }
                }
            }

            return response; 
        } 
        
        // Select Event Owner
        public async Task<Response> SelectEventOwner(int eventID)
        {
            var sqlstatement = "SELECT userID FROM dbo.Events WHERE eventID = @eventID";
            var response = new Response();
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
                            response.data = owner;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event count";

                    }
                }
            }

            return response;  
        } 
        
        // Select Attendance
        public async Task<Response> SelectAttendance(int eventID)
        {
            var sqlstatement = "SELECT showAttendance FROM dbo.Events WHERE eventID = @eventID";
            var response = new Response();
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
                            response.data = reader.GetInt32(reader.GetOrdinal("showAttendance"));
                            response.isSuccessful = true;
                        }
                    }
                    else
                    {
                        response.errorMessage = "Error Getting Event Date Created";

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
        
        
        
        
        
        
        
        
        
        
}

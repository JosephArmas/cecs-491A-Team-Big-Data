using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.UsersDB;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB
{
    public class PinsSqlDAO : DbContext, IPinDBInserter, IPinDBSelecter, IPinDBUpdater, IDBSelectPinOwner, IPinDBDeleter
    {
        private readonly String _connectionString;
        public PinsSqlDAO(DbContextOptions<PinsSqlDAO> options) : base(options)
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
        }



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


        //------------------------------------------------------------------------
        // IPinDBInserter
        //------------------------------------------------------------------------

        public async Task<Response> InsertNewPin(Pin pin)
        {
            var sql = "INSERT INTO dbo.Pins (userID,lat,lng,pinType,\"description\",userLastModified)" +
                "values(@ID, @lat, @lng, @type, @d, @mod)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", pin.UserID));
            command.Parameters.Add(new SqlParameter("@lat", pin.Lat));
            command.Parameters.Add(new SqlParameter("@lng", pin.Lng));
            command.Parameters.Add(new SqlParameter("@type", pin.PinType));
            command.Parameters.Add(new SqlParameter("@d", pin.Description));
            command.Parameters.Add(new SqlParameter("@mod", pin.UserID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Unique Key Constraint"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Pin already exists on this location.";
            }
            else if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage += ", {failed: ExecuteSqlCommand}";
            }
            else
            {
                result.IsSuccessful = true;
            }
            return result;
        }


        //------------------------------------------------------------------------
        // IPinDBSelecter
        //------------------------------------------------------------------------

        public async Task<DataResponse<List<PinResponse>>> SelectPinTable()
        {
            var result = new DataResponse<List<PinResponse>>();
            List<PinResponse> pins = new List<PinResponse>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sqlStatement = "SELECT * FROM dbo.Pins";

                    var command = new SqlCommand(sqlStatement, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                    if (reader.HasRows)
                    {
                        int pinID = 0;
                        int userID = 0;
                        String lat = "";
                        String lng = "";
                        int pinType = 0;
                        String description = "";
                        int disabled = 0;
                        DateTime dateCreated = new DateTime(2000, 1, 1);
                        int ordinal = reader.GetOrdinal("pinID");
                        if (!reader.IsDBNull(ordinal))
                        {
                            pinID = reader.GetInt32(ordinal);
                        }
                        ordinal = reader.GetOrdinal("userID");
                        if (!reader.IsDBNull(ordinal))
                        {
                            userID = reader.GetInt32(ordinal);
                        }
                        ordinal = reader.GetOrdinal("lat");
                        if (!reader.IsDBNull(ordinal))
                        {
                            lat = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("lng");
                        if (!reader.IsDBNull(ordinal))
                        {
                            lng = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("description");
                        if (!reader.IsDBNull(ordinal))
                        {
                            description = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("disabled");
                        if (!reader.IsDBNull(ordinal))
                        {
                            disabled = reader.GetInt32(ordinal);
                        }
                        ordinal = reader.GetOrdinal("dateCreated");
                        if (!reader.IsDBNull(ordinal))
                        {
                            dateCreated = reader.GetDateTime(ordinal);
                        }
                        ordinal = reader.GetOrdinal("pinType");
                        if (!reader.IsDBNull(ordinal))
                        {
                            pinType = reader.GetInt32(ordinal);
                        }
                        pins.Add(new PinResponse(pinID, userID, lat, lng, pinType, description, dateCreated.ToString()));
                    }
                    await reader.CloseAsync();
                    result.Data = pins;
                    result.IsSuccessful = true;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = e.Message;
                return result;
            }

        }

        public async Task<DataResponse<List<PinResponse>>> SelectEnabledPins()
        {
          
            var result = new DataResponse<List<PinResponse>>();
            List<PinResponse> pins = new List<PinResponse>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sqlStatement = "SELECT * FROM dbo.Pins";

                    var command = new SqlCommand(sqlStatement, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            int pinID = 0;
                            int userID = 0;
                            String lat = "";
                            String lng = "";
                            int pinType = 0;
                            String description = "";
                            int disabled = 0;
                            DateTime dateCreated = new DateTime(2000, 1, 1);
                            int ordinal = reader.GetOrdinal("pinID");
                            if (!reader.IsDBNull(ordinal))
                            {
                                pinID = reader.GetInt32(ordinal);
                            }
                            ordinal = reader.GetOrdinal("userID");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userID = reader.GetInt32(ordinal);
                            }
                            ordinal = reader.GetOrdinal("lat");
                            if (!reader.IsDBNull(ordinal))
                            {
                                lat = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("lng");
                            if (!reader.IsDBNull(ordinal))
                            {
                                lng = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("description");
                            if (!reader.IsDBNull(ordinal))
                            {
                                description = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("disabled");
                            if (!reader.IsDBNull(ordinal))
                            {
                                disabled = reader.GetInt32(ordinal);
                            }
                            ordinal = reader.GetOrdinal("dateCreated");
                            if (!reader.IsDBNull(ordinal))
                            {
                                dateCreated = reader.GetDateTime(ordinal);
                            }
                            ordinal = reader.GetOrdinal("pinType");
                            if (!reader.IsDBNull(ordinal))
                            {
                                pinType = reader.GetInt32(ordinal);
                            }
                            if (disabled == 0)
                            {
                                pins.Add(new PinResponse(pinID, userID, lat, lng, pinType, description, dateCreated.ToString()));
                            }
                        }
                    }
                    await reader.CloseAsync();
                    result.Data = pins;
                    result.IsSuccessful = true;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = e.Message;
                return result;
            }
        }



        //------------------------------------------------------------------------
        // IPinDBUpdater
        //------------------------------------------------------------------------

        public async Task<Response> UpdatePinContent(int pinID, int userID, string description)
        {
            var sql = "UPDATE dbo.Pins SET \"description\" = @desc, dateLastModified = @date, userLastModified = @user WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@desc", description));
            command.Parameters.Add(new SqlParameter("@date", DateTime.UtcNow));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Update Pin Content successfully for user";
            }
            return result;
        }

        public async Task<Response> UpdatePinType(int pinID, int userID, int pinType)
        {
            var sql = "UPDATE dbo.Pins SET pinType = @t, dateLastModified = @date, userLastModified = @user WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@t", pinType));
            command.Parameters.Add(new SqlParameter("@date", DateTime.UtcNow));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Update Pin Type successfully for user";
            }
            return result;
        }


        public async Task<Response> UpdatePinToDisabled(int pinID, int userID)
        {
            var sql = "UPDATE dbo.Pins SET \"disabled\" = 1, dateLastModified = @date, userLastModified = @user WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@date", DateTime.UtcNow));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.ErrorMessage = "Update Pin To Disabled successfully for user";
            }
            return result;
        }

        public async Task<DataResponse<int>> GetPinOwner(int pinID)
        {
            //var tcs = new TaskCompletionSource<Response>();
            var result = new DataResponse<int>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Creates an Insert SQL statements using the collumn names and values given
                var sql = "SELECT userID FROM dbo.Pins Where pinID = @p";
                try
                {
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add(new SqlParameter("@p", pinID));
                    int id = (int)command.ExecuteScalar();
                    if (id > 0)
                    {
                        result.Data = id;
                        result.IsSuccessful = true;
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                //tcs.SetResult(result);
                return result;
            }
        }


        //------------------------------------------------------------------------
        // IPinDBDeleter
        //------------------------------------------------------------------------

        public async Task<Response> DeletePinsLinkedToUser(int userID)
        {
            var sql = "DELETE FROM dbo.Pins WHERE userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var response = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (response.ErrorMessage.Equals("Nothing Affected"))
            {
                response.IsSuccessful = true;
                response.ErrorMessage = "No Pins to delete.";
            }
            else if (response.IsSuccessful)
            {
                response.IsSuccessful = true;
                response.ErrorMessage = "Users Pins successfully deleted.";
            }

            return response;
        }

        public async Task<Response> DeletePinFromTable(int pinID)
        {
            var sql = "DELETE FROM dbo.Pins WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Delete Pin From Table successfully for user";
            }
            return result;
        }

    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class PinsSqlDAO : DbContext, IPinDBInserter, IPinDBSelecter, IPinDBUpdater, IDBSelectPinOwner
    {
        private readonly String _connectionString;
        public PinsSqlDAO(DbContextOptions<PinsSqlDAO> options) : base(options)
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
        }

        public PinsSqlDAO(string connectionString)
        {
            _connectionString = connectionString;
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


        //------------------------------------------------------------------------
        // IPinDBInserter
        //------------------------------------------------------------------------

        public async Task<Response> InsertNewPin(Pin pin)
        {
            var sql = "INSERT INTO dbo.Pins (userID,lat,lng,pinType,\"description\",userLastModified)" +
                "values(@ID, @lat, @lng, @type, @d, @mod)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", pin._userID));
            command.Parameters.Add(new SqlParameter("@lat", pin._lat));
            command.Parameters.Add(new SqlParameter("@lng", pin._lng));
            command.Parameters.Add(new SqlParameter("@type", pin._pinType));
            command.Parameters.Add(new SqlParameter("@d", pin._description));
            command.Parameters.Add(new SqlParameter("@mod", pin._userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: ExecuteSqlCommand}";
                return result;
            }
            else
            {
                result.isSuccessful = true;
            }
            return result;
        }


        //------------------------------------------------------------------------
        // IPinDBSelecter
        //------------------------------------------------------------------------

        public async Task<DataResponse<List<PinResponse>>> SelectPinTable()
        {
            //var tcs = new TaskCompletionSource<DataResponse<List<Pin>>>();
            var result = new DataResponse<List<PinResponse>>();
            List<PinResponse> pins = new List<PinResponse>();
            string sqlStatement = "SELECT * FROM dbo.Pins";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                    {
                        // read through all rows
                        while (reader.Read())
                        {
                            int pinID = 0;
                            int userID = 0;
                            String lat = "";
                            String lng = "";
                            int pinType = 0;
                            String description = "";
                            int disabled = 0;
                            int completed = 0;
                            String dateCreated = "";
                            pinID = reader.GetInt32(0);
                            userID = reader.GetInt32(1);
                            lat = reader.GetString(2);
                            lng = reader.GetString(3);
                            pinType = reader.GetInt32(4);
                            description = reader.GetString(5);
                            disabled = reader.GetInt32(6);
                            completed = reader.GetInt32(7);
                            dateCreated = reader.GetDateTime(8).ToString();
                            pins.Add(new PinResponse(pinID, userID, lat, lng, pinType, description, disabled, completed, dateCreated));
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                if (pins.Count > 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "Returning List of Pins";
                    result.data = pins;
                }
                else if (pins.Count == 0 && result.errorMessage.Equals("SqlCommand Passed"))
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Empty List of Pins";
                    result.data = pins;
                }
            }

            return result;
        }


        //------------------------------------------------------------------------
        // IPinDBUpdater
        //------------------------------------------------------------------------

        public async Task<Response> UpdatePinToComplete(int pinID, int userID)
        {
            var sql = "UPDATE dbo.Pins SET completed = 1, dateLastModified = @date, userLastModified = @user WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if (result.isSuccessful)
            {
                result.isSuccessful = true;
                result.errorMessage = "Update Pin To Complete successfully for user";
            }
            return result;
        }

        public async Task<Response> UpdatePinContent(int pinID, int userID, string description)
        {
            var sql = "UPDATE dbo.Pins SET \"description\" = @desc, dateLastModified = @date, userLastModified = @user WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@desc", description));
            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if (result.isSuccessful)
            {
                result.isSuccessful = true;
                result.errorMessage = "Update Pin Content successfully for user";
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
            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if (result.isSuccessful)
            {
                result.isSuccessful = true;
                result.errorMessage = "Update Pin Type successfully for user";
            }
            return result;
        }


        public async Task<Response> UpdatePinToDisabled(int pinID, int userID)
        {
            var sql = "UPDATE dbo.Pins SET \"disabled\" = 1, dateLastModified = @date, userLastModified = @user WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if (result.isSuccessful)
            {
                result.errorMessage = "Update Pin To Disabled successfully for user";
            }
            return result;
        }

        public Task<Response> GetPinOwner(int pinID)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
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
                        result.data = id;
                        result.isSuccessful = true;
                    }
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }
    }
}

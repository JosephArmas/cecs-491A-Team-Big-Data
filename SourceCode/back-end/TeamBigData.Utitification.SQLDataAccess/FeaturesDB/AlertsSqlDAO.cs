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
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Alerts;


namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB
{
    public class AlertsSqlDAO : DbContext, IAlertDBInserter, IAlertDBSelecter, IAlertDBUpdater
    {
        private readonly String _connectionString;
        public AlertsSqlDAO(DbContextOptions<AlertsSqlDAO> options) : base(options)
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
        public async Task<Response> InsertAlert(Alert alert)
        {
            var sql = "INSERT INTO dbo.Pins (userID,lat,lng,pinType,\"description\")" +
                "values(@ID, @lat, @lng, @type, @d)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", alert.UserID));
            command.Parameters.Add(new SqlParameter("@lat", alert.Lat));
            command.Parameters.Add(new SqlParameter("@lng", alert.Lng));
            command.Parameters.Add(new SqlParameter("@type", alert.PinType));
            command.Parameters.Add(new SqlParameter("@d", alert.Description));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage += ", {failed: ExecuteSqlCommand}";
                return result;
            }
            else
            {
                result.IsSuccessful = true;
            }
            return result;
        }

        public async Task<DataResponse<List<Alert>>> SelectAlertTable()
        {
            var result = new DataResponse<List<Alert>>();
            List<Alert> alerts = new List<Alert>();
            string sqlStatement = "SELECT * FROM dbo.Alerts";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int alertID = reader.GetInt32(0);
                            int userID = reader.GetInt32(1);
                            String lat = reader.GetString(2);
                            String lng = reader.GetString(3);
                            int pinType = reader.GetInt32(4);
                            String description = reader.GetString(5);
                            int read = reader.GetInt32(7);
                            string dateCreated = reader.GetDateTime(8).ToString();
                            alerts.Add(new Alert(alertID, userID, lat, lng, pinType, description, read, dateCreated));
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                if (alerts.Count > 0)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "Returning List of Alerts";
                    result.Data = alerts;
                }
                else if (alerts.Count == 0 && result.ErrorMessage.Equals("SqlCommand Passed"))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Alerts don't exist";
                    result.Data = alerts;
                }
            }

            return result;
        }

        public async Task<Response> MarkAsRead(int alertID, int userID)
        {
            var sql = "UPDATE dbo.Alerts SET read = 1, dateTime = @date, userID = @user WHERE alertID = @a";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@a", alertID));
            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request";
            }
            else if (result.IsSuccessful)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "";
            }
            return result;
        }

        public async Task<Response> UpdateAlertContent(int alertID, int userID, string description)
        {
            var sql = "UPDATE dbo.Alerts SET \"description\" = @desc, dateTime = @date, userID = @user WHERE alertID = @a";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@a", alertID));
            command.Parameters.Add(new SqlParameter("@desc", description));
            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request;";
            }
            else if (result.IsSuccessful)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "";
            }
            return result;
        }

        public async Task<Response> UpdatePinType(int alertID, int userID, int pinType)
        {
            var sql = "UPDATE dbo.Alerts SET pinType = @t, dateTime= @date, userID = @user WHERE alertID = @a";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@a", alertID));
            command.Parameters.Add(new SqlParameter("@t", pinType));
            command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@user", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request";
            }
            else if (result.IsSuccessful)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "";
            }
            return result;
        }
    }
}

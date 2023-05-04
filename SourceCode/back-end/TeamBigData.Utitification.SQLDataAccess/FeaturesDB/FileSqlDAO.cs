using Microsoft.Data.SqlClient;
using TeamBigData.Utification.ErrorResponse;
using Microsoft.EntityFrameworkCore;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB
{
    public class FileSqlDAO : DbContext, IDBUploadPinPic, IDBUploadProfilePic, IDBDownloadPinPic, IDBDownloadProfilePic, IDBDeletePinPic, IDBDeleteProfilePic
    {
        private readonly String _connectionString;

        public FileSqlDAO(DbContextOptions<FileSqlDAO> options) : base(options)
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
        }

        public FileSqlDAO(string connectionString)
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
        public Task<DataResponse<int>> GetPinOwner(int pinID)
        {
            var tcs = new TaskCompletionSource<DataResponse<int>>();
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
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<Response> UploadPinPic(String key, int pinID)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            var sql = "Insert into dbo.PinPic(\"key\", pinID) values (@k, @ID)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@k", key));
            command.Parameters.Add(new SqlParameter("@ID", pinID));
            return ExecuteSqlCommand(connection, command);
        }

        public Task<Response> UploadProfilePic(String key, int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            var sql = "Insert into dbo.ProfilePic(\"key\", userID) values (@k, @ID)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@k", key));
            command.Parameters.Add(new SqlParameter("@ID", userID));
            return ExecuteSqlCommand(connection, command);
        }
        public Task<DataResponse<String>> DownloadPinPic(int pinID)
        {
            var tcs = new TaskCompletionSource<DataResponse<String>>();
            var result = new DataResponse<String>();
            String id = "";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Creates an Insert SQL statements using the collumn names and values given
                var sql = "SELECT \"key\" FROM dbo.PinPic Where pinID = @p";
                try
                {
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add(new SqlParameter("@p", pinID));
                    id = (String)command.ExecuteScalar();
                    if (!(id is null))
                    {
                        if (id.Length > 0)
                        {
                            result.Data = id;
                            result.IsSuccessful = true;
                        }
                    }
                    else
                    {
                        id = "";
                        result.Data = id;
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
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<DataResponse<String>> DownloadProfilePic(int userID)
        {
            var tcs = new TaskCompletionSource<DataResponse<String>>();
            var result = new DataResponse<String>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Creates an Insert SQL statements using the collumn names and values given
                var sql = "SELECT \"key\" FROM dbo.ProfilePic Where userID = @ID";
                try
                {
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add(new SqlParameter("@ID", userID));
                    String id = (String)command.ExecuteScalar();
                    if (id is null)
                    {
                        id = "";
                        result.Data = id;
                    }
                    else if (id.Length > 0)
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
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<Response> DeletePinPic(int pinID)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            var sql = "Delete from dbo.PinPic where pinID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", pinID));
            return ExecuteSqlCommand(connection, command);
        }

        public Task<Response> DeleteProfilePic(int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            var sql = "Delete from dbo.ProfilePic where userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            return ExecuteSqlCommand(connection, command);
        }
    }
}

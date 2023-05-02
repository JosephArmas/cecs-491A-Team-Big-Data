using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions;

namespace TeamBigData.Utification.SQLDataAccess.UserhashDB
{
    public class UserhashSqlDAO : DbContext, IUserhashDBInserter
    {
        private readonly String _connectionString;

        public UserhashSqlDAO(DbContextOptions<UserhashSqlDAO> options) : base(options)
        {

            _connectionString = this.Database.GetDbConnection().ConnectionString;
        }

        public UserhashSqlDAO(string connectionString)
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

        public async Task<Response> InsertUserHash(String userHash, int userID)
        {
            var insertSql = "INSERT into dbo.UserHash(userHash, \"userID\") values(@hash, @ID)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@hash", userHash));
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.ErrorMessage += $", {{failed: ExecuteSqlCommand, connectionstring:{_connectionString} }}";
            }
            else
            {
                result.IsSuccessful = true;
            }
            return result;
        }
    }
}

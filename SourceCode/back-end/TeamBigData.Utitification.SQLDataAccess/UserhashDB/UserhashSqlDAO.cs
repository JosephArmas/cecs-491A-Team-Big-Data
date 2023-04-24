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

        public async Task<Response> InsertUserHash(String userHash, int userID)
        {
            var insertSql = "INSERT into dbo.UserHash(userHash, \"userID\") values(@hash, @ID)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@hash", userHash));
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.errorMessage += $", {{failed: ExecuteSqlCommand, connectionstring:{_connectionString} }}";
            }
            else
            {
                result.isSuccessful = true;
            }
            return result;
        }
    }
}

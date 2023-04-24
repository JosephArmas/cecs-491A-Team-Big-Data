using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;

namespace TeamBigData.Utification.SQLDataAccess.LogsDB
{
    public class LogsSqlDAO : DbContext, ILogsDBInserter
    {
        private readonly String _connectionString;

        public LogsSqlDAO(DbContextOptions<LogsSqlDAO> options) : base(options)
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

        public async Task<Response> InsertLog(Log log)
        {
            var insertSql = "Insert into dbo.Logs (CorrelationID, LogLevel, UserHash, [Event], Category, [Message]) values (@id, @level, @hash, @event, @category, @message)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@id", log._correlationID));
            command.Parameters.Add(new SqlParameter("@level", log._logLevel));
            command.Parameters.Add(new SqlParameter("@hash", log._user));
            command.Parameters.Add(new SqlParameter("@event", log._event));
            command.Parameters.Add(new SqlParameter("@category", log._category));
            command.Parameters.Add(new SqlParameter("@message", log._message));
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

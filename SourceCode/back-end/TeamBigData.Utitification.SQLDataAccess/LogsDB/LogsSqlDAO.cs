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

        public LogsSqlDAO(string connectionString)
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

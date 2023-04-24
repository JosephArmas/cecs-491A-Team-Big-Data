using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;

namespace TeamBigData.Utification.Logging
{
    public class Logger : ILogger
    {
        private readonly ILogsDBInserter _logsDBInserter;
        public Logger(ILogsDBInserter logsDBInserter)
        {
            _logsDBInserter = logsDBInserter;
        }
        public async Task<Response> Logs(Log log)
        {
            Response result = new Response();
            //Validation
            if (!(log._logLevel.Equals("Info") || log._logLevel.Equals("Warning") || log._logLevel.Equals("Debug") || log._logLevel.Equals("Error")))
            {
                result.errorMessage = "Error: The log did not contain a proper log level";
                result.isSuccessful = false;
                return result;
            }
            if (!(log._category.Equals("View") || log._category.Equals("Business") || log._category.Equals("Server") || log._category.Equals("Data") || log._category.Equals("Data Store")))
            {
                result.errorMessage = "Error: The log did not contain a proper category";
                result.isSuccessful = false;
                return result;
            }
            result = await _logsDBInserter.InsertLog(log).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed _logsDBInserter.InsertLog}";
                return result;
            }
            else
            {
                result.isSuccessful = true;
            }

            return result;
        }
    }
}
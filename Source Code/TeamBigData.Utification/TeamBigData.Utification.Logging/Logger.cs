using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.Logging
{
    public class Logger : ILogger
    {
        private readonly IDAO _dao;
        public Logger(IDAO dao)
        {
            _dao = dao;
        }
        public async Task<Response> Log(Log log)
        {
            Response result = new Response();
            //Validation
            if (!(log._logLevel.Equals("Info") || log._logLevel.Equals("Warning") || log._logLevel.Equals("Debug")
                || log._logLevel.Equals("Error")))
            {
                result.errorMessage = "Error: The log did not contain a proper log level";
                result.isSuccessful = false;
                return result;
            }
            if (!(log._category.Equals("View") || log._category.Equals("Business")
                    || log._category.Equals("Server") || log._category.Equals("Data") || log._category.Equals("Data Store")))
            {
                result.errorMessage = "Error: The log did not contain a proper category";
                result.isSuccessful = false;
                return result;
            }
            String insertSql = "Insert into dbo.Logs (CorrelationID, LogLevel, UserHash, Event, Category, Message) values (" + log._correlationID + ", '" + log._logLevel +
                "', '" + log._user + "', '" + log._event + "', '" + log._category + "', '" + log._message + "');";
            Response logReturn = await _dao.Execute(insertSql).ConfigureAwait(false);
            result.isSuccessful = false;
            if (logReturn.isSuccessful)
            {
                result.isSuccessful = true;
            }
            else
            {
                result.errorMessage = logReturn.errorMessage;
            }

            return result;
        }
    }
}
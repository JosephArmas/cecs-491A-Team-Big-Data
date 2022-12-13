using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

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
            if (!(log.GetLogLevel().Equals("Info") || log.GetLogLevel().Equals("Warning") || log.GetLogLevel().Equals("Debug")
                || log.GetLogLevel().Equals("Error")))
            {
                result.errorMessage = "Error: The log did not contain a proper log level";
                result.isSuccessful = false;
                return result;
            }
            if (!(log.GetCategory().Equals("View") || log.GetCategory().Equals("Business")
                    || log.GetCategory().Equals("Server") || log.GetCategory().Equals("Data") || log.GetCategory().Equals("Data Store")))
            {
                result.errorMessage = "Error: The log did not contain a proper category";
                result.isSuccessful = false;
                return result;
            }
            String insertSql = "Insert into dbo.Logs (CorrelationID, LogLevel, \"User\", Event, Category, Message) values (" + log.GetCorrelationID() + ", '" + log.GetLogLevel() +
                "', '" + log.GetUser() + "', '" + log.GetEvent() + "', '" + log.GetCategory() + "', '" + log.GetMessage() + "');";
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
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Logging
{

    public class Logger : ILogger
    {
        private readonly IDAO _dao;
        public Logger(IDAO dao)
        {
            _dao = dao;
        }
        public async Task<Response> Log(String message)
        {
            Response result = new Response();
            //Validation
            if (message == null)
            {
                result.isSuccessful = false;
                result.errorMessage = "No object was given to log";
                return result;
            }
            string msg = message;
            if (msg.Contains("SELECT") || msg.Contains("UPDATE") || msg.Contains("DELETE"))
            {
                result.errorMessage = "Error: INSERT is the only valid request";
                result.isSuccessful = false;
                return result;
            }
            if (!(msg.Contains("Info") || msg.Contains("Warning") || msg.Contains("Debug")
                || msg.Contains("Error")))
            {
                result.errorMessage = "Error: The log did not contain a proper log level";
                result.isSuccessful = false;
                return result;
            }
            if (!(msg.Contains("View") || msg.Contains("Business")
                    || msg.Contains("Server") || msg.Contains("Data") || msg.Contains("Data Store")))
            {
                result.errorMessage = "Error: The log did not contain a proper category";
                result.isSuccessful = false;
                return result;
            }
            Response logReturn = await _dao.Execute(message).ConfigureAwait(false);
            result.isSuccessful = false;
            if (logReturn.isSuccessful)
            {
                result.isSuccessful = true;
            }

            return result;
        }
    }
}
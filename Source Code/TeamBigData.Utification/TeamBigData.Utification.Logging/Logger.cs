using DataAccess;
using Domain;
using Logging.Abstractions;

namespace Logging.Implementations
{
    public class Logger : ILogger
    {
        private readonly IDAO _dao;
        public Logger(IDAO dao)
        {
            _dao = dao;
        }
        public async Task<Result> Log(object message)
        {
            Result result = new Result();
            //Validation
            if (message == null) 
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No object was given to log";
                return result;
            }
            if (_dao.GetType() == typeof(string))
            {
                string msg = message.ToString();
                if (msg.Contains("SELECT") || msg.Contains("UPDATE") || msg.Contains("DELETE"))
                {
                    result.ErrorMessage = "Error: INSERT is the only valid request";
                    result.IsSuccessful = false;
                    return result;
                }
                if (!(msg.Contains("Info") || msg.Contains("Warning") || msg.Contains("Debug")
                    || msg.Contains("Error")))
                {
                    result.ErrorMessage = "Error: The log did not contain a proper log level";
                    result.IsSuccessful = false;
                    return result;
                }
                if (!(msg.Contains("View") || msg.Contains("Business")
                     || msg.Contains("Server") || msg.Contains("Data") || msg.Contains("Data Store")))
                {
                    result.ErrorMessage = "Error: The log did not contain a proper category";
                    result.IsSuccessful = false;
                    return result;
                }
            }
            Result logReturn = await _dao.Execute(message).ConfigureAwait(false);
            result.IsSuccessful = false;
            if (logReturn.IsSuccessful)
            {
                result.IsSuccessful = true;
            }

            return result;
        }
    }
}
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
        public Result Log(object message)
        {
            Result result = new Result();
            //Validation
            if (message.GetType() == typeof(string))
            {
                string msg = message.ToString();
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
            //Result logReturn = _dao.Execute(message);
            result.IsSuccessful = true;

            return result;
        }
    }
}
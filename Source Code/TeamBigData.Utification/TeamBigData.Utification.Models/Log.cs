namespace TeamBigData.Utification.Models
{
    /// <summary>
    /// Logs format
    /// </summary>
    public class Log
    {
        public int _correlationID { get; private set; }
        public string _logLevel { get; private set; }
        public string _user { get; private set; }
        public string _event { get; private set; }
        public string _category { get; private set; }
        public string _message { get; private set; }

        public Log(int correlationID, string logLevel, string user, string eventName, string category, string message)
        {
            _correlationID = correlationID;
            _logLevel = logLevel;
            _user = user;
            _event = eventName;
            _category = category;
            _message = message;
        }
    }
}
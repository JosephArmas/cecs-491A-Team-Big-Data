using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class Log
    {
        private int _correlationID;
        private string _logLevel;
        private string _user;
        private string _event;
        private string _category;
        private string _message;

        public Log(int correlationID, string logLevel, string user, string eventName, string category, string message)
        {
            _correlationID = correlationID;
            _logLevel = logLevel;
            _user = user;
            _event = eventName;
            _category = category;
            _message = message;
        }

        public int GetCorrelationID()
        {
            return _correlationID;
        }

        public string GetLogLevel()
        {
            return _logLevel;
        }

        public string GetUser()
        {
            return _user;
        }

        public string GetEvent()
        {
            return _event;
        }

        public string GetCategory()
        {
            return _category;
        }

        public string GetMessage()
        {
            return _message;
        }

        public void SetCorrelationID(int correlationID)
        {
            _correlationID = correlationID;
        }

        public void SetLogLevel(string logLevel)
        {
            _logLevel = logLevel;
        }

        public void SetUser(string user)
        {
            _user = user;
        }

        public void SetEvent(string eventName)
        {
            _event = eventName;
        }

        public void SetCategory(string category)
        {
            _category = category;
        }

        public void SetMessage(string message)
        {
            _message = message;
        }
    }
}

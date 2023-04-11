using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBigData.Utification.Models
{
    public class Pin
    {
        public int _pinID { get; set; }
        public int _userID { get; set; }
        public string _lat { get; set; }
        public string _lng { get; set; }
        public int _pinType { get; set; }
        public string _description { get; set; }
        public int _disabled { get; set; }
        public int _completed { get; set; }
        public string _dateTime { get; set; }

        public Pin()
        {
            _pinID = 0;
            _userID = 0;
            _lat = "";
            _lng = "";    
            _pinType = 0;
            _description = "";
            _disabled = 0;
            _completed = 0;
            _dateTime = "";
        }

        public Pin(int pinID, int userID, string lat, string lng, int pinType, string description, int disabled, int completed, string dateTime)
        {
            _pinID = pinID;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = pinType;
            _description = description;
            _disabled = disabled;
            _completed = completed;
            _dateTime = dateTime;
        }
        public Pin(int userID, string lat, string lng, int pinType, string description)
        {
            _pinID = 0;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = pinType;
            _description = description;
            _disabled = 0;
            _completed = 0;
            _dateTime = "";
        }

        public Pin(string lat, string lng, int pinType, string description, int completed, string dateTime)
        {
            _pinID = 0;
            _userID = 0;
            _lat = lat;
            _lng = lng;
            _pinType = pinType;
            _description = description;
            _disabled = 0;
            _completed = 0;
            _dateTime = dateTime;
        }

        public string ToString()
        { 
            return "{ _pinID: " + _pinID + ", _userID: " + _userID + ", _lat: " + _lat + ", _lng: " + _lng + ", _pinType: " + _pinType + "_description: " + _description + ", _disabled: " + _disabled + ", compelted: " + _completed + ", dateTime: " + _dateTime + " }";
        }
    }
}

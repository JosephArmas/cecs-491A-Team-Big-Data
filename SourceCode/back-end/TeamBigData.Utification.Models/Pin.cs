using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBigData.Utification.Models
{
    public class Pin
    {
        public int _pinID { get; set; }
        public int _userID { get; set; }
        public String _lat { get; set; }
        public String _lng { get; set; }
        public int _pinType { get; set; }
        public String _description { get; set; }
        public int _disabled { get; set; }
        public int _completed { get; set; }
        public String _dateCreated { get; set; }
        public String _dateLastModified { get; set; }
        public int _userLastModified { get; set; }

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
            _dateCreated = "";
            _dateLastModified = "";
            _userLastModified = 0;
        }

        public Pin(int pinID, int userID, String lat, String lng, int pinType, String description, int disabled, int completed, String dateCreated, String dateLastModified, int userLastModified)
        {
            _pinID = pinID;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = pinType;
            _description = description;
            _disabled = disabled;
            _completed = completed;
            _dateCreated = dateCreated;
            _dateLastModified = dateLastModified;
            _userLastModified = userLastModified;
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
            _dateCreated = DateTime.Now.ToString();
            _dateLastModified = DateTime.Now.ToString();
            _userLastModified = userID;
        }

        public string ToString()
        {
            return $"{{ _pinID: {_pinID}, _userID: {_userID}, _lat: {_lat}, _lng: {_lng}, _pinType: {_pinType}, _description: {_description}, _disabled: {_disabled}, _completed: {_completed}, _dateCreated: {_dateCreated}, _dateLastModified: {_dateLastModified}, _userLastModified: {_userLastModified} }}";

        }
    }
}

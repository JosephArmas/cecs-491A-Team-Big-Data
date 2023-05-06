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
<<<<<<< HEAD
            DateCreated = new DateTime(2000,1,1);
            DateLastModified = new DateTime(2000, 1, 1);
=======
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
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
        }

        public Pin(int pinID, int userID, String lat, String lng, int pinType, String description, int disabled, int completed, String dateCreated, String dateLastModified, int userLastModified)
        {
<<<<<<< HEAD
            PinID = pinID;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            Disabled = disabled;
            DateCreated = dateCreated;
            DateLastModified = dateLastModified;
            UserLastModified = userLastModified;
        }
        public Pin(int pinID, int userID, String lat, String lng, int pinType, String description, int disabled, DateTime dateCreated)
        {
            PinID = pinID;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            Disabled = disabled;
            DateCreated = dateCreated;
            DateLastModified = new DateTime(2000, 1, 1);
        }
        public Pin(int userID, string lat, string lng, int pinType, string description)
        {
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            DateCreated = new DateTime(2000, 1, 1);
            DateLastModified = new DateTime(2000, 1, 1);
=======
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
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
        }

        public string ToString()
        {
            return $"{{ _pinID: {_pinID}, _userID: {_userID}, _lat: {_lat}, _lng: {_lng}, _pinType: {_pinType}, _description: {_description}, _disabled: {_disabled}, _completed: {_completed}, _dateCreated: {_dateCreated}, _dateLastModified: {_dateLastModified}, _userLastModified: {_userLastModified} }}";

        }
    }
}

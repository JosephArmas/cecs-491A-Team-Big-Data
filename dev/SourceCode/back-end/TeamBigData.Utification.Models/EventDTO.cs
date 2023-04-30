
namespace TeamBigData.Utification.Models
{
    public class EventDTO
    {
        public int _eventID { get; set; }
        public int _userID { get; set; }
        public int _count { get; set; }
        public string _title { get; set; }
        public string _description { get; set; }
        public DateTime _postDate { get; set; }
        public double _lng { get; set; }
        
        public double _lat { get; set; }
        
        // Ctor assign mapping
        
        public EventDTO(string title, string description, int userID, double lat, double lng)
        {
            _userID = userID;
            _title = title;
            _description = description;
            _lat = lat;
            _lng = lng;
        }

        public EventDTO (string title, string description, double lat, double lng, int eventID)
        {
            _title = title;
            _description = description;
            _eventID = eventID;
            _lat = lat;
            _lng = lng;

        }
            

        public EventDTO(string title, string description)
        {
            _title = title;
            _description = description;
        }
        public EventDTO(string title, string description, int eventId)
        {
            _title = title;
            _description = description;
            _eventID = eventId;
        }
        
        public EventDTO(int eventId)
        {
            _eventID = eventId;
        }
        public string ToString()
        {
            return "EventID: " + _eventID + ", UserID: " + _userID + ", Title: " + _title + ", Description: " +
                   _description + ", PostDate:" + _postDate + ", Latitude: " + _lat + ", Longitude: " + _lng;
        }
        
    }
    
    
}

namespace TeamBigData.Utification.Models
{
    public class EventDTO
    {
        public int _pinID { get; set; }
        public int _userID { get; set; }
        public int _count { get; set; }
        public string _title { get; set; }
        public string _description { get; set; }
        public DateTime _postDate { get; set; }
        
        // Ctor assign mapping
        public EventDTO(int pinID, int userID, int count, string title, string description, DateTime postDate)
        {
            _pinID = pinID;
            _userID = userID;
            _count = count;
            _title = title;
            _description = description;
            _postDate = postDate;
        }

        public EventDTO(string title, string description)
        {
            _title = title;
            _description = description;
        }
        
        public string ToString()
        {
            return "PinID: " + _pinID + ", UserID: " + _userID + ", Title: " + _title + ", Description: " +
                   _description + ", PostDate:" + _postDate;
        }
        
    }
    
    
}

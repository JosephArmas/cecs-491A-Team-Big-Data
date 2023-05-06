
namespace TeamBigData.Utification.Models
{
    public class EventDTO
    {
        public int EventID { get; set; }
        public int UserID { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public double Lng { get; set; }

        public double Lat { get; set; }

        // Ctor assign mapping

        public EventDTO(string title, string description, int userID, double lat, double lng)
        {
            UserID = userID;
            Title = title;
            Description = description;
            Lat = lat;
            Lng = lng;
        }

        public EventDTO(string title, string description, double lat, double lng, int eventID, int count)
        {
            Title = title;
            Description = description;
            EventID = eventID;
            Lat = lat;
            Lng = lng;
            Count = count;

        }


        public EventDTO(string title, string description)
        {
            Title = title;
            Description = description;
        }
        public EventDTO(string title, string description, int eventId)
        {
            Title = title;
            Description = description;
            EventID = eventId;
        }

        public EventDTO(int eventId)
        {
            EventID = eventId;
        }
        public string ToString()
        {
            return "EventID: " + EventID + ", UserID: " + UserID + ", Title: " + Title + ", Description: " +
                   Description + ", PostDate:" + PostDate + ", Latitude: " + Lat + ", Longitude: " + Lng;
        }

    }


}

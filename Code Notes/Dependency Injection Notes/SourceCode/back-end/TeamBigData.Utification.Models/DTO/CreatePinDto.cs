namespace TeamBigData.Utification.Models.DTO
{
    public class CreatePinDto
    {
        public int _userID { get; set; }
        public string _lat { get; set; }
        public string _lng { get; set; }
        public int _pinType { get; set; }
        public string _description { get; set; }
    }
}

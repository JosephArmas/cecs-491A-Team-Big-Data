namespace TeamBigData.Utification.Models
{
    public class Report
    {
        public double Rating { get; private set; }
        public int ReportedUser { get; private set; }
        public int ReportingUser { get; private set; }
        public string? Feedback { get; private set; }


        public Report(double rating, int reportedUser, int reportingUser, string? feedback)
        {
            Rating = rating;
            ReportedUser = reportedUser;
            ReportingUser = reportingUser;
            Feedback = feedback;
        }

        public override string ToString()
        {
            return "{_rating: " + Rating + ", _reportedUserID: " + ReportedUser + ", _reportingUserID: " + ReportingUser + ", _feedback: " + Feedback + "}";
        }
    }
}
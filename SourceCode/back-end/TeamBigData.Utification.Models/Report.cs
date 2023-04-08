using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class Report
    {
        public double _rating { get; set; }
        public int _reportedUserID { get; set; }
        public int _reportingUserID { get; set; }
        public string? _feedback { get; set; }
    
        public Report(double rating, int reportedUserID, int reportingUserID, string? feedback)
        {
            _rating = rating;
            _reportedUserID = reportedUserID;
            _reportingUserID = reportingUserID;
            _feedback = feedback;
        }
 
        public override string ToString()
        {
            return "{_rating: " + _rating + ", _reportedUserID: " + _reportedUserID + ", _reportingUserID: " + _reportingUserID +", _feedback: " + _feedback + "}";
        }
    }
}

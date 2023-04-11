using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class Report
    {
        public double _rating { get; private set; }
        public int _reportedUser { get; private set; }
        public int _reportingUser { get; private set; }
        public string? _feedback { get; private set; }

    
        public Report(double rating, int reportedUser, int reportingUser, string? feedback)
        {
            _rating = rating;
            _reportedUser = reportedUser;
            _reportingUser = reportingUser;
            _feedback = feedback;
        }
 
        public override string ToString()
        {
            return "{_rating: " + _rating + ", _reportedUserID: " + _reportedUser + ", _reportingUserID: " + _reportingUser +", _feedback: " + _feedback + "}";
        }
    }
}

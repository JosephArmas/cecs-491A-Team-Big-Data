using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class RecoveryRequests
    {
        public String Username {  get; set; }
        public String Timestamp { get; set; }

        public RecoveryRequests(String username, String timestamp)
        {
            Username = username;
            Timestamp = timestamp;
        }
    }
}

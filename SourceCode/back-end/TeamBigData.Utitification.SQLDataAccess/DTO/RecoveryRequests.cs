using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class RecoveryRequests
    {
        public String _username {  get; set; }
        public String _timestamp { get; set; }

        public RecoveryRequests(String username, String timestamp)
        {
            _username = username;
            _timestamp = timestamp;
        }
    }
}

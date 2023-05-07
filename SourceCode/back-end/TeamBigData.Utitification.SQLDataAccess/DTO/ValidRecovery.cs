using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class ValidRecovery
    {
        public String Password { get; set; }
        public String Salt { get; set; }
        public ValidRecovery(string password, string salt)
        {
            Password = password;
            Salt = salt;
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class ValidRecovery
    {
        public String _password { get; set; }
        public String _salt { get; set; }
        public ValidRecovery(string password, string salt)
        {
            _password = password;
            _salt = salt;
        }
    }
    
}

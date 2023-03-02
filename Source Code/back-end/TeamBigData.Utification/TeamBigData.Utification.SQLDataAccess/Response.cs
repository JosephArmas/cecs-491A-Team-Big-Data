using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class Response
    {
        public bool isSuccessful;
        public string errorMessage;

        public String ToString()
        {
            return isSuccessful.ToString() + ": " + errorMessage;
        }
    }
}

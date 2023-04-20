using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.ErrorResponse
{
    public class DataResponse<T>
    {
        public bool isSuccessful;
        public string errorMessage;
        public T data;

        public DataResponse()
        {
            isSuccessful = false;
            errorMessage = "";
        }

        public String ToString()
        {
            return isSuccessful.ToString() + ": " + errorMessage;
        }
    }
}

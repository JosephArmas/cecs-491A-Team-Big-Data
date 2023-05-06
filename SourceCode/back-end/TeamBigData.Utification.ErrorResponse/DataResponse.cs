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
        public DataResponse(bool isSuccessful, String errorMessage)
        {
            this.isSuccessful = isSuccessful;
            this.errorMessage = errorMessage;
        }
        public DataResponse(bool isSuccessful, string errorMessage, T data)
        {
            this.isSuccessful = isSuccessful;
            this.errorMessage = errorMessage;
            this.data = data;
        }

        public String ToString()
        {
            return isSuccessful.ToString() + ": " + errorMessage;
        }
    }
}

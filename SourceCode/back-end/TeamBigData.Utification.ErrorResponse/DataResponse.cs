using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.ErrorResponse
{
    public class DataResponse<T>
    {
        public bool IsSuccessful;
        public string ErrorMessage;
        public T Data;

        public DataResponse()
        {
            IsSuccessful = false;
            ErrorMessage = "";
        }
        public DataResponse(bool isSuccessful, String errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }
        public DataResponse(bool isSuccessful, string errorMessage, T data)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public String ToString()
        {
            return $"{{ IsSuccessful: {IsSuccessful}, ErrorMessage: {ErrorMessage}, Data: {Data} }}";
        }
    }
}

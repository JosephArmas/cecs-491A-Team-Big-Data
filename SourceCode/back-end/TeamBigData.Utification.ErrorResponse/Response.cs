namespace TeamBigData.Utification.ErrorResponse
{
    public class Response
    {
        public bool IsSuccessful;
        public string ErrorMessage;
        public Object Data;

        public Response()
        {
            IsSuccessful = false;
            ErrorMessage = "";
        }

        public Response(bool success, String message)
        {
            IsSuccessful = success;
            ErrorMessage = message;
            Data = null;
        }

        public String ToString()
        {
            return $"{{ IsSuccessful: {IsSuccessful}, ErrorMessage: {ErrorMessage} }}";
        }
    }
}
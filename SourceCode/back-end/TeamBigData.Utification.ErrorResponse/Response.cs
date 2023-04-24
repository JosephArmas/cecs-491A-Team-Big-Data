namespace TeamBigData.Utification.ErrorResponse
{
    public class Response
    {
        public bool isSuccessful;
        public string errorMessage;
        public Object data;

        public Response()
        {
            isSuccessful = false;
            errorMessage = "";
        }

        public Response(bool success, String message)
        {
            isSuccessful = success;
            errorMessage = message;
        }

        public String ToString()
        {
            return isSuccessful.ToString() + ": " + errorMessage;
        }
    }
}
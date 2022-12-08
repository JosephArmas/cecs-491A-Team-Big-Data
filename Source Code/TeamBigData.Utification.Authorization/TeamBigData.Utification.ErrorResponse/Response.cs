namespace TeamBigData.Utification.ErrorResponse
{
    public class Response
    {
        public bool isSuccessful;
        public string errorMessage;
        public object data;

        public Response()
        {
            isSuccessful = false;
            errorMessage = "";
            data = 0;
        }

        public String ToString()
        {
            return isSuccessful.ToString() + ": " + errorMessage;
        }
    }
}
﻿namespace TeamBigData.Utification.ErrorResponse
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
        }

        public String ToString()
        {
            return IsSuccessful.ToString() + ": " + ErrorMessage;
        }
    }
}
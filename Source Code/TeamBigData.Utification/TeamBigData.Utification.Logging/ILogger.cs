using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Logging
{ 
    public interface ILogger
    {
    Task<Response> Log(String message);
    }
}
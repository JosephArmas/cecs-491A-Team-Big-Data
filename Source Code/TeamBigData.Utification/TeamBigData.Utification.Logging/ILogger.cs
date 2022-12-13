using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Logging
{ 
    public interface ILogger
    {
    Task<Response> Log(Log log);
    }
}
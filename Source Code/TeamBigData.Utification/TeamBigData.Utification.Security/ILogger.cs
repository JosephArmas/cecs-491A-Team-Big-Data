using Domain;

namespace Logging.Abstractions
{
    public interface ILogger
    {
        Task<Result> Log(object message);
    }
}

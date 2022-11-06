using Domain;

namespace Logging.Abstractions
{
    public interface ILogger
    {
        Result Log(object message);
    }
}

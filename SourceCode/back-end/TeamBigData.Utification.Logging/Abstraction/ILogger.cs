using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Logging.Abstraction
{
    public interface ILogger
    {
        Task<Response> Logs(Log log);
    }
}

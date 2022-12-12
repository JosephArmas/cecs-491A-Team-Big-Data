using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Logging
{
    public interface ILogger
    {
        Task<Response> Log(string message);
    }
}

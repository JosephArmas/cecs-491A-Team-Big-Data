using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utitification.SQLDataAccess.Abstractions
{
    public interface IDAO
    {
        Task<Response> Execute(object req);
    }
}

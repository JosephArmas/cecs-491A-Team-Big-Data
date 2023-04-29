using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDAO
    {
        public Task<Response> Execute(String req);
    }
}

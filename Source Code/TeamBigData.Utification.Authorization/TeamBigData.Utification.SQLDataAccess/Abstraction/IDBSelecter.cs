using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstraction
{
    public interface IDBSelecter
    {
        public Task<Response> SelectTable(List<object> list, object req);
    }
}

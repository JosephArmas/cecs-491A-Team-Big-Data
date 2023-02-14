using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess
{
    public interface IDBInserter
    {
        public Task<Response> Insert(String tableName, String[] values);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess
{
    public  interface IDBCounter
    {
        public Task<Response> Count(String tableName, String countedCollumn, String[] collumnNames, String[] parameters);
    }
}

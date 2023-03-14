using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess
{
    public interface IDBDeleter
    {
        public Task<Response> Delete(String tableName, String[] collumnNames, String[] values);
    }
}

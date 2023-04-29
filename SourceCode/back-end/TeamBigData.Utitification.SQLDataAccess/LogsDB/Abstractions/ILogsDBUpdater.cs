using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions
{
    public interface ILogsDBUpdater
    {
        public Task<Response> UpdateLogsTest(String sqlCommand);
    }
}

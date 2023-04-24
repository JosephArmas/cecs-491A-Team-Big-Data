using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions
{
    public interface ILogsDBInserter
    {
        public Task<Response> InsertLog(Log log);
    }
}

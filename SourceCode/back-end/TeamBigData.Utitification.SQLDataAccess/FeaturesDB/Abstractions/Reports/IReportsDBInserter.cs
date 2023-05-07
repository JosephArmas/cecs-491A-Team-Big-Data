using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports
{
    public interface IReportsDBInserter
    {
        public Task<Response> InsertUserReportAsync(Report report);
    }
}

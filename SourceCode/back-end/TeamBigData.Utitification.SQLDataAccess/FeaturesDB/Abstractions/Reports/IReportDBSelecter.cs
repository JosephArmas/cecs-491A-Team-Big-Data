using System.Data;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports
{
    public interface IReportsDBSelecter
    {
        public Task<DataResponse<DataSet>> SelectUserReportsAsync(UserProfile userProfile);
        public Task<DataResponse<Tuple<double, int>>> SelectNewReputationAsync(Report report);
    }
}

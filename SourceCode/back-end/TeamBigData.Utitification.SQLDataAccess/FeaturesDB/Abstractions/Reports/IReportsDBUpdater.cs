using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports
{
    public interface IReportsDBUpdater
    {
        public Task<Response> UpdateFeedback();
    }
}

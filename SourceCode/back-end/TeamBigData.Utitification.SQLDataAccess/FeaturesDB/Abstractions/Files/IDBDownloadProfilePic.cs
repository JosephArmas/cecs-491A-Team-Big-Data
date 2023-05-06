using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files
{
    public interface IDBDownloadProfilePic
    {
        public Task<Response> DownloadProfilePic(int userID);
    }
}

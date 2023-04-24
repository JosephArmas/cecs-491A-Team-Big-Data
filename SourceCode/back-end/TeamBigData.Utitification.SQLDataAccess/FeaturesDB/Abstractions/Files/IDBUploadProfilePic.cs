using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files
{
    public interface IDBUploadProfilePic
    {
        public Task<Response> UploadProfilePic(String key, int userID);
    }
}

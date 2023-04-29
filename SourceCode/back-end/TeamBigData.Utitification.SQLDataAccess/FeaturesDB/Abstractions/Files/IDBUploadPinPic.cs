using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files
{
    public interface IDBUploadPinPic
    {
        public Task<Response> UploadPinPic(String key, int pinID);
    }
}

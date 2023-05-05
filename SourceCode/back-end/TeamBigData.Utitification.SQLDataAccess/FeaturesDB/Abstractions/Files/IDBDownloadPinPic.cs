using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files
{
    public interface IDBDownloadPinPic
    {
        public Task<DataResponse<String>> DownloadPinPic(int pinID);
    }
}

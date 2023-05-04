using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files
{
    public interface IDBDeletePinPic
    {
        public Task<Response> DeletePinPic(int pinId);
    }
}

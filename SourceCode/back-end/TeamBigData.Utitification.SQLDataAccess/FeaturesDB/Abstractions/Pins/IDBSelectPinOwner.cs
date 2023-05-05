using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins
{
    public interface IDBSelectPinOwner
    {
        public Task<DataResponse<int>> GetPinOwner(int pinID);
    }
}

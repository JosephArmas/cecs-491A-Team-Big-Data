using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.DTO;

namespace TeamBigData.Utification.Manager.Abstractions
{
    public interface ILogin
    {
        public Task<DataResponse<AuthenticateUserResponse>> LoginUser(String email, String password, String userhash);
    }
}

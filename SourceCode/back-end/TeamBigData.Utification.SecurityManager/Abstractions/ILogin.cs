using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Manager.Abstractions
{
    public interface ILogin
    {
        public Task<DataResponse<UserProfile>> LoginUser(String email, byte[] encryptedPassword, Encryptor encryptor, UserProfile userProfile);
    }
}

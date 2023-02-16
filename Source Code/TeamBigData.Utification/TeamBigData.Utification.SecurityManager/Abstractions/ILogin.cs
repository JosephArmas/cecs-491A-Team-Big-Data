using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Manager.Abstractions
{
    public interface ILogin
    {
        public Response LoginUser(String email, byte[] encryptedPassword, Encryptor encryptor, ref UserAccount userAccount, ref UserProfile userProfile);
    }
}

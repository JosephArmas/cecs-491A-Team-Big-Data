using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Manager.Abstractions
{
    public interface IRegister
    {
        public Task<Response> RegisterUser(String email, String password, String userhash);
    }
}

using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Manager.Abstractions
{
    public interface ILogout
    {
        public Task<Response> LogOutUser(ref UserAccount userAccount, ref UserProfile userProfile);
    }
}

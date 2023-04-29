using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Manager.Abstractions
{
    public interface ILogout
    {
        public Task<DataResponse<UserProfile>> LogOutUser(UserProfile userProfile, String userhash);
    }
}

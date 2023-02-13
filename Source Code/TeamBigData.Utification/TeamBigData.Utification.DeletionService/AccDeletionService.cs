using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.DeletionService
{
    public class AccDeletionService : IDeletionService
    {
        private UserProfile _userProfile;
        public AccDeletionService(UserProfile user)
        {
            _userProfile = user;
        }
        public Task<Response> DeleteFeatures()
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteProfile()
        {
            throw new NotImplementedException();
        }
    }
}
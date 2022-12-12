using TeamBigData.Utification.Models;
using TeamBigData.Utification.UserManager.Abstraction;

namespace TeamBigData.Utification.UserManager
{
    public class UserManager : IUserManager
    {
        public UserProfile CreateUser(UserAccount userAccount)
        {
            var temp = new UserProfile("");
            return temp;
        }

        public bool DeleteUser()
        {
            throw new NotImplementedException();
        }

        public bool DisableUser()
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }
    }
}
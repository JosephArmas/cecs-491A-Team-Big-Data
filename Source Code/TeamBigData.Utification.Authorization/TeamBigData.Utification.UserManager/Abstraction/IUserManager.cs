using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.UserManager.Abstraction
{
    public interface IUserManager
    {
        UserProfile CreateUser(UserAccount userAccount); 
        // user profile should have userid linked 
        // don't want to return userAccount - security concern when not using password 
        // registration is just this 

        bool DeleteUser();

        bool UpdateUser(UserProfile userProfile);
        // force to update their profile on the first log in 
        // fills out both user account and user profile

        bool DisableUser();
    }
}

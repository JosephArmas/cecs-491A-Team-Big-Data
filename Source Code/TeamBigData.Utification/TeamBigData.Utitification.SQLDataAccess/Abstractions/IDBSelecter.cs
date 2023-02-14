using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBSelecter
    {
        public UserProfile SelectUserProfile(String username);
        public List<UserProfile> SelectUserProfileTable();
        public UserAccount SelectUserAccount(String username);
        public List<UserAccount> SelectUserAccountTable();
        public Response SelectLastUserID();
    }
}

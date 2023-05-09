using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions
{
    public interface IUsersDBUpdater
    {
        public Task<Response> UpdateRecoveryFulfilled(int userID);
        public Task<Response> UpdateUserPassword(int userID, String password, String salt);
        public Task<Response> UpdatePinCompletionTallyAsync(int userID, int completionTally);
        public Task<Response> UpdateUserRoleAsync(UserProfile userProfile);
        public Task<Response> UpdateUserReputationAsync(int user, double newReputation);
        public Task<DataResponse<int>> UpdateServiceRole(int userid);
        public Task<DataResponse<int>> UpdateRemoveServiceRole(int userid);
    }
}

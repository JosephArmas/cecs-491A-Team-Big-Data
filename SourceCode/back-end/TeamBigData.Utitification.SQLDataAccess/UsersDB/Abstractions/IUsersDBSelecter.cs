using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.DTO;

namespace TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions
{
    public interface IUsersDBSelecter
    {
        public Task<DataResponse<UserAccount>> SelectUserAccount(String username);
        public Task<DataResponse<UserProfile>> SelectUserProfile(int userID);
        public Task<DataResponse<List<RecoveryRequests>>> SelectRecoveryRequestsTable();
        public Task<DataResponse<ValidRecovery>> SelectRecoveryUser(int userID);
        public Task<Response> SelectUserProfileRole(int userID);
        public Task<Response> SelectUserHash(int userID);
        public Task<Response> SelectUserID(string email);
    }
}

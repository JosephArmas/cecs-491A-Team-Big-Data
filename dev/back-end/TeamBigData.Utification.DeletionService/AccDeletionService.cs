﻿using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.DeletionService
{
    public class AccDeletionService : IDeletionService
    {
        private UserProfile _userProfile;
        public AccDeletionService(UserProfile user)
        {
            _userProfile = user;
        }
        public async Task<Response> DeletePIIFeatures()
        {
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Features;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            var err = "User feature data could not be deleted";
            var result = await DeletePII(connectionString, err, 1).ConfigureAwait(false);
            return result;
        }

        public async Task<Response> DeletePIIProfile()
        {
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            var err = "User profile data could not be deleted";
            var result = await DeletePII(connectionString,err,0).ConfigureAwait(false);
            return result;
        }

        public async Task<Response> DeletePII(String connString, String err,int piData)
        {
            var result = new Response();
            result.isSuccessful = false;
            var userDao = new SQLDeletionDAO(connString);
            if (piData == 0)
            {
                result = await userDao.DeleteUser(_userProfile).ConfigureAwait(false);
            }
            else
            {
                result = await userDao.DeleteFeatureInfo(_userProfile).ConfigureAwait(false);
            }
            if (result.isSuccessful == false)
            {
                result.errorMessage = err;
            }
            return result;
        }
    }
}
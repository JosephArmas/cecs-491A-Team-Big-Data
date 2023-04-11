﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.DTO;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBInserter
    {
        public Task<Response> InsertUser(UserAccount user);
        public Task<Response> InsertUserProfile(UserProfile profile);
        public Task<Response> InsertUserHash(String userHash, int userID);
        public Task<Response> IncrementUserAccountDisabled(UserAccount userAccount);
        public Task<Response> InsertPin(CreatePinDto pin);
    }
}

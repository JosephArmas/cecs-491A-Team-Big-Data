﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBInserter
    {
        public Task<Response> InsertUser(UserAccount user);
        public Task<Response> InsertUserProfile(UserProfile profile);
        public Task<Response> InsertUserHash(String userHash, int userID);
        public Task<Response> IncrementUserAccountDisabled(UserAccount userAccount);
        public Task<Response> InsertPin(Pin pin);
        // public Task<Response> InsertEvent(string title, string description, int userID, float lat, float lng);
        public Task<Response> InsertEvent(EventDTO eventDto);
        public Task<Response> InsertJoinEvent(int eventID, int userID);
    }
}
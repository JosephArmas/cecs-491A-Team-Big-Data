﻿using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files
{
    public interface IDBDeleteProfilePic
    {
        public Task<Response> DeleteProfilePic(int userID);
    }
}

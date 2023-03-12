﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBCounter
    {
        public Task<Response> Count(String tableName, String countedCollumn, String[] collumnNames, String[] parameters);
        public Task<Response> CountSalt(String salt);
        public Task<Response> CountAll(String tableName, String countedCollumn);
        public Task<Response> CountUserLoginAttempts(String username);
    }
}
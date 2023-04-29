using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBCounter
    {
        public Task<Response> CountSalt(String salt);
        public Task<Response> CountUserLoginAttempts(int userId);
    }
}

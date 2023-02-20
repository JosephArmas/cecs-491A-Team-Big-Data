using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBDeleter
    {
        public Task<Response> DeleteUser(String user);
        public Task<Response> DeleteFeatureInfo(String user);
    }
}

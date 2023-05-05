using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions
{
    public interface IUserhashDBUpdater
    {
        public Task<Response> UnlinkUserhashFrom(int userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports
{
    public interface IReportsDBDeleter
    {
        public Task<Response> DeleteUserReport();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports
{
    public interface IReportsDBSelecter
    {
        public Task<Response> SelectUserReportsAsync(UserProfile userProfile);
        public Task<Response> SelectNewReputationAsync(Report report);
    }
}

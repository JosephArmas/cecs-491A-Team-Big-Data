using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports
{
    public interface IReportsDBSelecter
    {
        public Task<Response> SelectUserReportsAsync(UserProfile userProfile);
        public Task<Response> SelectNewReputationAsync(Report report);
    }
}

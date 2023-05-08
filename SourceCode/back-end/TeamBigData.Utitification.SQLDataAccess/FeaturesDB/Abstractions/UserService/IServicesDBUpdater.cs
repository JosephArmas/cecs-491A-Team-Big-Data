using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices
{
    public interface IServicesDBUpdater
    {
        public Task<Response> UpdateProvider(ServiceModel serv);

        public Task<Response> UpdateRequestAccept(RequestModel request);

        public Task<Response> UpdateRequestDeny(RequestModel request);

        public Task<Response> UpdateRequestCancel(ServiceModel serv);
        public Task<Response> DeleteProvider(ServiceModel serv);
    }
}

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
        public Task<DataResponse<int>> UpdateProvider(ServiceModel serv);

        public Task<DataResponse<int>> UpdateRequestAccept(RequestModel request);

        public Task<DataResponse<int>> UpdateRequestDeny(RequestModel request, int userid);

        public Task<DataResponse<int>> UpdateRequestCancel(ServiceModel serv);
        public Task<DataResponse<int>> DeleteProvider(ServiceModel serv);
    }
}

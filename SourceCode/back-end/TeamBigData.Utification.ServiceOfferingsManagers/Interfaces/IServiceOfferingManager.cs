using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.ServiceOfferingsManagers.Interfaces
{
    public interface IServiceOfferingManager
    {
        public Task<DataResponse<int>> unregister(ServiceModel Serv);
        public Task<DataResponse<int>> CreateService(ServiceModel Serv);
        public Task<DataResponse<int>> UpdateService(ServiceModel Serv);
    }
}

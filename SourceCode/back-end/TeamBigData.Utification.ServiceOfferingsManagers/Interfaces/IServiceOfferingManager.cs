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
        public Task<Response> unregister(ServiceModel Serv);
        public Task<Response> CreateService(ServiceModel Serv);
        public Task<Response> UpdateService(ServiceModel Serv);
    }
}

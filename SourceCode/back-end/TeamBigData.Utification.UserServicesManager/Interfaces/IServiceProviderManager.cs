using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.UserServicesManager.Interfaces
{
    public interface IServiceProviderManager
    {
        public Task<Response> unregister(ServModel Serv);
        public Task<Response> CreateService(ServModel Serv);
        public Task<Response> UpdateService(ServModel Serv);
    }
}

using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.ServiceOfferingsServices
{
    public class ServiceOfferingService
    {
        private readonly IServicesDBInserter _servicesDBInserter;
        private readonly IServicesDBSelecter _servicesDBSelecter;
        private readonly IServicesDBUpdater _servicesDBUpdater;
        private readonly IUsersDBUpdater _usersDBUpdater;

        public ServiceOfferingService(IServicesDBInserter servicesDBInserter, IServicesDBSelecter servicesDBSelecter, IServicesDBUpdater servicesDBUpdater, IUsersDBUpdater usersDBUpdater)
        {
            _servicesDBInserter = servicesDBInserter;
            _servicesDBSelecter = servicesDBSelecter;
            _servicesDBUpdater = servicesDBUpdater;
            _usersDBUpdater = usersDBUpdater;
        }
        Response serviceresult = new Response();
        private async Task<Response> GetTotalServices()
        {
            var response = new Response();

            response = await _servicesDBSelecter.GetServiceCount().ConfigureAwait(false);
            if (response.data.GetType() == typeof(int))
            {
                if ((int)response.data >= -1)
                {
                    response.isSuccessful = true;
                    response.errorMessage = "There is space";
                }
                else
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Services is full";
                    return response;
                }
            }
            return response;
        }
        private async Task<Response> UpdateRoleToService(int userid)
        {
            var response = new Response();
            response = await _usersDBUpdater.UpdateServiceRole(userid).ConfigureAwait(false);
            if (response.data.GetType() == typeof(int))
            {
                if ((int)response.data == 1)
                {
                    response.isSuccessful = true;
                    response.errorMessage = "Role has been updated";
                }
                else
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Failed to update role";
                    return response;
                }
            }
            return response;
        }

        private async Task<Response> UpdateRoleToRegular(int userid)
        {
            var response = new Response();
            response = await _usersDBUpdater.UpdateRemoveServiceRole(userid).ConfigureAwait(false);
            if (response.data.GetType() == typeof(int))
            {
                if ((int)response.data == 1)
                {
                    response.isSuccessful = true;
                    response.errorMessage = "Role has been updated";
                }
                else
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Failed to update role";
                    return response;
                }
            }
            return response;
        }


        public async Task<Response> unregister(ServiceModel serv)
        {
            var response = new Response();
            response = await _servicesDBUpdater.DeleteProvider(serv).ConfigureAwait(false);
            if (response.data.GetType() != typeof(int))
            {

                response.isSuccessful = false;
                response.errorMessage = "Incorrect Return Type";
                return response;
            }

            if ((int)response.data != 1)
            {
                response.isSuccessful = false;
                response.errorMessage = "Service Failed to Delete";
                return response;
            }

            response.isSuccessful = true;
            response.errorMessage = "Service Successfully Deleted";

            var role = await UpdateRoleToRegular(serv.CreatedBy).ConfigureAwait(false);

            if (role.isSuccessful == true)
            {
                response.errorMessage += " and Role was updated to regular";
            }
            else
            {
                response.isSuccessful = false;
                response.errorMessage += " and Role was not updated to regular";
            }

            return response;
        }


        public async Task<Response> CreateServiceProv(ServiceModel serv)
        {
            var response = new Response();

            var servicesCount = await GetTotalServices().ConfigureAwait(false);

            if (!servicesCount.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage = "Too many service users";
                return response;
            }

            response = await _servicesDBInserter.InsertProvider(serv).ConfigureAwait(false);

            if (response.data.GetType() != typeof(int))
            {
                response.isSuccessful = false;
                response.errorMessage = "Invalid return from data access";
                return response;
            }

            if ((int)response.data != 1)
            {
                response.isSuccessful = false;
                response.errorMessage = "Could not insert into database";
                return response;
            }

            response.isSuccessful = true;
            response.errorMessage = "Successfully inserted into database";

            var role = await UpdateRoleToService(serv.CreatedBy).ConfigureAwait(false);

            if ((int)role.data != 1)
            {
                response.isSuccessful = false;
                response.errorMessage = "Failed to update user role";
            }

            return response;
        }


        public async Task<Response> UpdateServiceProv(ServiceModel serv)
        {
            var response = new Response();

            response = await _servicesDBUpdater.UpdateProvider(serv).ConfigureAwait(false);

            response.isSuccessful = true;
            response.errorMessage = "Successfully Updated Service";

            if (response.data.GetType() != typeof(int))
            {
                response.isSuccessful = false;
                response.errorMessage = "Data access error";
                return response;
            }
            if ((int)response.data != 1)
            {
                response.isSuccessful = true;
                response.errorMessage = "Failed to update service";
                return response;
            }
            
            return response;
        }
    }
}

using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.ServiceOfferingsServices
{
    public class ServiceOfferingService
    {
        private readonly IServicesDBInserter _servicesDBInserter;
        private readonly IServicesDBSelecter _servicesDBSelecter;
        private readonly IServicesDBUpdater _servicesDBUpdater;
        private readonly IUsersDBUpdater _usersDBUpdater;

        public ServiceOfferingService(ServicesSqlDAO servicesSqlDAO, UsersSqlDAO usersSqlDAO)
        {
            _servicesDBInserter = servicesSqlDAO;
            _servicesDBSelecter = servicesSqlDAO;
            _servicesDBUpdater = servicesSqlDAO;
            _usersDBUpdater = usersSqlDAO;
        }
        Response serviceresult = new Response();
        private async Task<DataResponse<int>> GetTotalServices()
        {
            
            var response = new DataResponse<int>();

            response = await _servicesDBSelecter.GetServiceCount().ConfigureAwait(false);
            if (response.Data.GetType() == typeof(int))
            {
                if ((int)response.Data >= -1)
                {
                    response.IsSuccessful = true;
                    response.ErrorMessage = "There is space";
                }
                else
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Services is full";
                    return response;
                }
            }
            return response;

        }
        private async Task<DataResponse<int>> UpdateRoleToService(int userid)
        {
            var response = new DataResponse<int>();
            response = await _usersDBUpdater.UpdateServiceRole(userid).ConfigureAwait(false);
            if (response.Data.GetType() == typeof(int))
            {
                if ((int)response.Data == 1)
                {
                    response.IsSuccessful = true;
                    response.ErrorMessage = "Role has been updated";
                }
                else
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Failed to update role";
                    return response;
                }
            }
            return response;

        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        private async Task<DataResponse<int>> UpdateRoleToRegular(int userid)
        {
            var response = new DataResponse<int>();
            response = await _usersDBUpdater.UpdateRemoveServiceRole(userid).ConfigureAwait(false);
            if (response.Data.GetType() == typeof(int))
            {
                if ((int)response.Data == 1)
                {
                    response.IsSuccessful = true;
                    response.ErrorMessage = "Role has been updated";
                }
                else
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Failed to update role";
                    return response;
                }
            }
            return response;

        }


        public async Task<DataResponse<int>> unregister(ServiceModel serv)
        {
            
            var response = new DataResponse<int>();
            response = await _servicesDBUpdater.DeleteProvider(serv).ConfigureAwait(false);
            if (response.Data.GetType() != typeof(int))
            {

                response.IsSuccessful = false;
                response.ErrorMessage = "Incorrect Return Type";
                return response;
            }

            if ((int)response.Data != 1)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Service Failed to Delete";
                return response;
            }

            response.IsSuccessful = true;
            response.ErrorMessage = "Service Successfully Deleted";

            var role = await UpdateRoleToRegular(serv.CreatedBy).ConfigureAwait(false);

            if (role.IsSuccessful == true)
            {
                response.ErrorMessage += " and Role was updated to regular";
            }
            else
            {
                response.IsSuccessful = false;
                response.ErrorMessage += " and Role was not updated to regular";
            }

            return response;

        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<int>> CreateServiceProv(ServiceModel serv)
        {
            var response = new DataResponse<int>();

            var servicesCount = await GetTotalServices().ConfigureAwait(false);

            if (!servicesCount.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Too many service users";
                return response;
            }

            response = await _servicesDBInserter.InsertProvider(serv).ConfigureAwait(false);

            if (response.Data.GetType() != typeof(int))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid return from data access";
                return response;
            }

            if (response.Data != 1)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Could not insert into database";
                return response;
            }

            response.IsSuccessful = true;
            response.ErrorMessage = "Successfully inserted into database";

            var role = await UpdateRoleToService(serv.CreatedBy).ConfigureAwait(false);

            if (role.Data != 1)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Failed to update user role";
            }

            return response;

        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<int>> UpdateServiceProv(ServiceModel serv)
        {
            
            var response = new DataResponse<int>();

            response = await _servicesDBUpdater.UpdateProvider(serv).ConfigureAwait(false);

            response.IsSuccessful = true;
            response.ErrorMessage = "Successfully Updated Service";

            if (response.Data.GetType() != typeof(int))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Data access error";
                return response;
            }
            if ((int)response.Data != 1)
            {
                response.IsSuccessful = true;
                response.ErrorMessage = "Failed to update service";
                return response;
            }

            return response;
        }
    }
}

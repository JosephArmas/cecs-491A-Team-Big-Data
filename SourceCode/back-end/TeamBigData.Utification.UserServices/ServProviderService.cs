using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.UserServices
{
    public class ServProviderService
    {

        private async Task<Response> GetTotalServices()
        {
            var response = new Response();
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False");

            response = await dao.GetServiceCount().ConfigureAwait(false);
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
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False");
            response = await dao.UpdateServiceRole(userid).ConfigureAwait(false);
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
                    response.errorMessage = "Error Updating Account Role";
                    return response;
                }
            }
            return response;
        }

        private async Task<Response> UpdateRoleToRegular(int userid)
        {
            var response = new Response();
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False");
            response = await dao.UpdateRemoveServiceRole(userid).ConfigureAwait(false);
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


        public async Task<Response> unregister(ServModel serv)
        {
            var response = new Response();
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False");
            response = await dao.DeleteProvider(serv).ConfigureAwait(false);
            if (response.data.GetType() == typeof(int))
            {
                if ((int)response.data == 1)
                {
                    response.isSuccessful = true;
                    response.errorMessage = "Service Successfully Deleted";
                    var role = await UpdateRoleToRegular(serv.CreatedBy).ConfigureAwait(false);
                    if(role.isSuccessful == true)
                    {
                        response.isSuccessful = true;
                        response.errorMessage += " and Role was updated to regular";
                    }
                    else
                    {
                        response.isSuccessful = false;
                        response.errorMessage += " and Role was not updated to regular";
                    }
                }
                else
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Service Failed to Delete";
                    return response;
                }
            }
            else
            {
                response.isSuccessful = false;
                response.errorMessage = "Incorrect Return Type";
            }
            return response;
        }


        public async Task<Response> CreateServiceProv(ServModel serv)
        {
            var response = new Response();
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False");

            var servicesCount = await GetTotalServices().ConfigureAwait(false);
            if (servicesCount.isSuccessful)
            {

                response = await dao.InsertProvider(serv).ConfigureAwait(false);

                if (response.data.GetType() == typeof(int))
                {
                    if ((int)response.data == 1)
                    {
                        response.isSuccessful = true;
                        response.errorMessage = "Successfully inserted into database";
                        var role = await UpdateRoleToService(serv.CreatedBy).ConfigureAwait(false);
                    }
                    else
                    {
                        response.isSuccessful = false;
                        response.errorMessage = "Could not insert into database";
                    }
                }
                else
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Incorrect Return Type";
                }
            }
            return response;
        }


        public async Task<Response> UpdateServiceProv(ServModel serv) 
        {
            var response = new Response();
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False");
            response = await dao.UpdateProvider(serv).ConfigureAwait(false);
            if (response.data.GetType() == typeof(int))
            {
                if ((int)response.data == 1)
                {
                    response.isSuccessful = true;
                    response.errorMessage = "Successfully Updated Service";
                }
                else if ((int)response.data > 1)
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Failed to update service";
                }
                else
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Failed to update service";
                }
            }
            return response;
        }
    }
}

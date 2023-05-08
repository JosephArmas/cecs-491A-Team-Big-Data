using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB
{
    public class ServicesSqlDAO : DbContext, IServicesDBInserter, IServicesDBSelecter, IServicesDBUpdater
    {
        #region UserServices

        private readonly String _connectionString;

        public ServicesSqlDAO(DbContextOptions<ServicesSqlDAO> options) : base(options)
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
        }


        #region Creation
        /// <summary>
        /// Checks to see of the active service accounts is less than the cap
        /// </summary>
        /// <returns>int of rows counted</returns> 

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> GetServiceCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Response();

                var selectSql = "CountServices";
                var command = new SqlCommand(selectSql, connection);
                command.CommandType = CommandType.StoredProcedure;
                //result.data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return result;
            }
        }



        /// <summary>
        /// Opens a connection to the database then fills out the stored procedure parameters
        /// </summary>
        /// <param name="serv">Service being inserted</param>
        /// <returns>int of rows inserted</returns>

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> InsertProvider(ServiceModel serv)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Response();

                var insertSql = "AddService";
                var command = new SqlCommand(insertSql, connection);
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@ServiceID", serv.ServiceID));
                command.Parameters.Add(new SqlParameter("@ServiceName", serv.ServiceName));
                command.Parameters.Add(new SqlParameter("@ServiceDesc", serv.ServiceDescription));
                command.Parameters.Add(new SqlParameter("@ServicePhone", serv.ServicePhone));
                command.Parameters.Add(new SqlParameter("@ServiceURL", serv.ServiceURL));
                command.Parameters.Add(new SqlParameter("@ServiceLat", serv.ServiceLat));
                command.Parameters.Add(new SqlParameter("@ServiceLong", serv.ServiceLong));
                command.Parameters.Add(new SqlParameter("@PinTypes", serv.PinTypes));
                command.Parameters.Add(new SqlParameter("@Distance", serv.Distance));
                command.Parameters.Add(new SqlParameter("@CreatedBy", serv.CreatedBy));
                command.Parameters.Add(new SqlParameter("@CreationDate", DateTime.UtcNow));

                //var result = await ExecuteSqlCommand(connection, command);
                try
                {
                    //result.data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Microsoft.Data.SqlClient.SqlException e)
                {
                    result.ErrorMessage = e.ToString();
                    result.IsSuccessful = false;
                    //result.data = 0;
                }
                return result;
            }

        }
        /// <summary>
        /// Updates the service that will no longer be used to be dissociated from the user
        /// </summary>
        /// <param name="serv">The Service that will be "deleted"</param>
        /// <returns>int of rows "deleted"</returns>

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> DeleteProvider(ServiceModel serv)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var deleteSql = "DeleteService";
                var command = new SqlCommand(deleteSql, connection);
                var result = new Response();
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.Add(new SqlParameter("@ServiceName", serv.ServiceName));
                command.Parameters.Add(new SqlParameter("@CreatedBy", serv.CreatedBy));

                //result.data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return result;
            }
        }
        /// <summary>
        /// Updates the service with revised information
        /// </summary>
        /// <param name="serv">The Service that will be updated</param>
        /// <returns>int of rows updated</returns>

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> UpdateProvider(ServiceModel serv)
        {
            var result = new Response();
            var connection = new SqlConnection(_connectionString);
            var updateSql = "UpdateService";
            var command = new SqlCommand(updateSql, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@ServiceName", serv.ServiceName));
            command.Parameters.Add(new SqlParameter("@ServiceDesc", serv.ServiceDescription));
            command.Parameters.Add(new SqlParameter("@ServicePhone", serv.ServicePhone));
            command.Parameters.Add(new SqlParameter("@ServiceURL", serv.ServiceURL));
            command.Parameters.Add(new SqlParameter("@ServiceLat", serv.ServiceLat));
            command.Parameters.Add(new SqlParameter("@ServiceLong", serv.ServiceLong));
            command.Parameters.Add(new SqlParameter("@PinTypes", serv.PinTypes));
            command.Parameters.Add(new SqlParameter("@Distance", serv.Distance));
            command.Parameters.Add(new SqlParameter("@Updater", serv.CreatedBy));
            using (connection)
            {
                connection.Open();


                //var result = await ExecuteSqlCommand(connection, command);
                //result.data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return result;
            }

        }
        #endregion

        #region Requests
        //Incomplete
        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> getnearbyservice(Pin pin, int dist)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Response();
                var selectSql = "GetNearbyServices";
                var command = new SqlCommand(selectSql, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RequestLat", pin.Lat));
                command.Parameters.Add(new SqlParameter("@RequestLong", pin.Lng));
                command.Parameters.Add(new SqlParameter("@Distance", dist));
                List<ArrayList> services = new List<ArrayList>();
                SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        services.Add(new ArrayList { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7), reader.GetInt32(8) });
                    }
                }
                await reader.CloseAsync();
                //result.data = services;
                return result;
            }
        }
        /// <summary>
        /// Inserts the service into the database
        /// </summary>
        /// <param name="serv">The Service that is being requested</param>
        /// <param name="pin">The pin that is requesting the service</param>
        /// <returns></returns>

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> InsertServiceReq(ServiceModel serv, Pin pin)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Response();
                var insertSql = "AddRequest";
                var command = new SqlCommand(insertSql, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@ServiceID", serv.ServiceID));
                command.Parameters.Add(new SqlParameter("@ServiceName", serv.ServiceName));
                command.Parameters.Add(new SqlParameter("@User", pin.UserID));
                command.Parameters.Add(new SqlParameter("@RequestLat", pin.Lat));
                command.Parameters.Add(new SqlParameter("@RequestLong", pin.Lng));
                command.Parameters.Add(new SqlParameter("@PinType", pin.PinType));


                //result.data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return result;
            }
        }
        /// <summary>
        /// Gets the requests for a service provider
        /// </summary>
        /// <returns>Returns all of the requests for a service provider</returns>
        /// <exception cref="NotImplementedException"></exception>

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> GetProviderRequests(ServiceModel serv)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = new Response();
                var selectSql = "GetProviderRequests";
                var command = new SqlCommand(selectSql, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ServiceID", serv.ServiceID));
                List<ArrayList> requests = new List<ArrayList>();
                SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        requests.Add(new ArrayList { reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6), reader.GetInt32(7) });
                    }
                }
                await reader.CloseAsync();
                //result.data = requests;
                return result;
            }
        }
        //Fix this it needs to have userprofile or something like that to have the userID

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> GetUserRequests(UserProfile user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = new Response();
                var selectSql = "GetUserRequests";
                var command = new SqlCommand(selectSql, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RequestedBy", user.UserID));
                List<ArrayList> requests = new List<ArrayList>();
                SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        requests.Add(new ArrayList { reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6), reader.GetInt32(7) });
                    }
                }
                await reader.CloseAsync();
                //result.data = requests;
                return result;
            }
        }
        /// <summary>
        /// Updates a singular request to be accepted
        /// </summary>
        /// <returns>int of rows updated</returns>

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> UpdateRequestAccept(RequestModel request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Response();
                var selectSql = "AcceptRequest";
                var command = new SqlCommand(selectSql, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RequestID", request.RequestID));
                command.Parameters.Add(new SqlParameter("@UserID", 8));
                //result.data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return result;
            }
        }

        public async Task<Response> UpdateRequestCancel(ServiceModel serv)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Response();
                var selectSql = "CancelRequest";
                var command = new SqlCommand(selectSql, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RequestID", 1));
                throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> UpdateRequestDeny(RequestModel request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Response();
                var selectSql = "DenyRequest";
                var command = new SqlCommand(selectSql, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RequestID", request.RequestID));
                command.Parameters.Add(new SqlParameter("@UserID", 8));
                //result.data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return result;
            }
        }
        #endregion
        #endregion
    }
}

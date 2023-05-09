using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;


namespace TeamBigData.Utification.SQLDataAccess.UsersDB
{
    public class UsersSqlDAO : DbContext, IUsersDBInserter, IUsersDBSelecter, IUsersDBUpdater, IUsersDBDeleter
    {
        private readonly String _connectionString;
        private readonly IConfiguration _configuration;
        public UsersSqlDAO(DbContextOptions<UsersSqlDAO> options, IConfiguration configuration) : base(options) 
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
            _configuration = configuration;
        }

        public UsersSqlDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        private async Task<Response> ExecuteSqlCommand(SqlConnection connection, SqlCommand command)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            //Executes the SQL Insert Statement using the Connection String provided
            try
            {
                connection.Open();
                var rows = command.ExecuteNonQuery();
                if (rows > 0)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "SqlCommand Passed";
                }
                else if (rows == 0)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "Nothing Affected";
                }
                connection.Close();
            }
            catch (SqlException s)
            {
                result.ErrorMessage = s.Message + ", {failed: command.ExecuteNonQuery}";
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message + ", {failed: command.ExecuteNonQuery}";
            }
            tcs.SetResult(result);
            return result;
        }


        //------------------------------------------------------------------------
        // IUsersDBInserter
        //------------------------------------------------------------------------

        public async Task<Response> InsertUserAccount(String email, String digest, String salt, String userhash)
        {
            var insertSql = "INSERT INTO dbo.Users (username, \"password\", \"disabled\", salt, userHash) values(@u, @p, 0, @s, @h)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@u", email));
            command.Parameters.Add(new SqlParameter("@p", digest));
            command.Parameters.Add(new SqlParameter("@s", salt));
            command.Parameters.Add(new SqlParameter("@h", userhash));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.ErrorMessage += ", {failed: ExecuteSqlCommand}";
            }
            else
            {
                result.IsSuccessful = true;
            }
            return result;
        }

        public async Task<Response> InsertUserProfile(int userId)
        {
            //Creates an Insert SQL statements using the collumn names and values given
            var insertSql = "INSERT into dbo.UserProfiles(userID, firstname, lastname, \"address\", birthday, reputation, tally, \"role\") values" +
                "(@uID, @n, @ln, @add, @bday, @reputation, @tally, @role)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@uID", userId));
            command.Parameters.Add(new SqlParameter("@n", ""));
            command.Parameters.Add(new SqlParameter("@ln", ""));
            command.Parameters.Add(new SqlParameter("@add", ""));
            command.Parameters.Add(new SqlParameter("@bday", (new DateTime(2000, 1, 1)).ToString()));
            command.Parameters.Add(new SqlParameter("@reputation", Convert.ToDecimal(_configuration["Reputation:DefaultReputation"])));
            command.Parameters.Add(new SqlParameter("@tally", SqlDbType.Int)).Value = 0;
            command.Parameters.Add(new SqlParameter("@role", "Regular User"));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage += ", {failed: ExecuteSqlCommand}";
            }
            else
            {
                result.IsSuccessful = true;
            }
            return result;
        }

        public async Task<Response> InsertRecoveryRequest(int userID, String digest, String salt)
        {
            String insertSql = "Insert into dbo.RecoveryRequests(userID, digest, salt) values (@ID, @d, @salt)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            command.Parameters.Add(new SqlParameter("@d", digest));
            command.Parameters.Add(new SqlParameter("@salt", salt));
            var response = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (response.ErrorMessage.Contains("conflicted with the FOREIGN KEY constraint \"RR_ForeignKey_01\""))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
            }
            return response;
        }


        //------------------------------------------------------------------------
        // IUsersDBSelecter
        //------------------------------------------------------------------------

        public async Task<DataResponse<UserAccount>> SelectUserAccount(string username)
        {
            var tcs = new TaskCompletionSource<DataResponse<UserAccount>>();
            var userAccount = new DataResponse<UserAccount>();
            string sqlStatement = "SELECT * FROM dbo.Users WHERE username = @u";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand(sqlStatement, connect);
                    command.Parameters.Add(new SqlParameter("@u", username));
                    await connect.OpenAsync();
                    using (var reader = command.ExecuteReader())
                    {
                        // read through all rows
                        while (reader.Read())
                        {
                            int userID = 0;
                            String userName = "";
                            String password = "";
                            String salt = "";
                            String userHash = "";
                            bool verified = false;

                            int ordinal = reader.GetOrdinal("userID");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userID = reader.GetInt32(ordinal);
                            }
                            ordinal = reader.GetOrdinal("username");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userName = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("password");
                            if (!reader.IsDBNull(ordinal))
                            {
                                password = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("disabled");
                            if (!reader.IsDBNull(ordinal))
                            {
                                if (reader.GetInt32(ordinal) < 3)
                                {
                                    verified = true;
                                }
                                else
                                {
                                    verified = false;
                                }
                            }
                            ordinal = reader.GetOrdinal("salt");
                            if (!reader.IsDBNull(ordinal))
                            {
                                salt = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("userHash");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userHash = reader.GetString(ordinal);
                            }
                            userAccount.Data = new UserAccount(userID, userName, password, salt, userHash, verified);
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    userAccount.ErrorMessage = s.Message + ", {failed: ExecuteReader}";
                }
                catch (Exception e)
                {
                    userAccount.ErrorMessage = e.Message + ", {failed: ExecuteReader}";
                }
            }
            if (userAccount.Data == null)
            {
                userAccount.IsSuccessful = false;
                userAccount.ErrorMessage += ", {failed: userAccount null}";
                return userAccount;
            }
            else if (userAccount.Data.UserID > 1000)
            {
                userAccount.IsSuccessful = true;
                userAccount.ErrorMessage = "UserAccount Found";
                return userAccount;
            }
            else
            {
                userAccount.IsSuccessful = false;
                userAccount.ErrorMessage = "No UserAccount Found";
                return userAccount;
            }
            
        }

        public async Task<DataResponse<UserProfile>> SelectUserProfile(int userID)
        {
            var result = new DataResponse<UserProfile>();
            var userProfile = new UserProfile();
            string sqlStatement = "SELECT * FROM dbo.UserProfiles WHERE userID = @ID";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    await connect.OpenAsync();
                    var command = new SqlCommand(sqlStatement, connect);
                    command.Parameters.Add(new SqlParameter("@ID", userID));
                    using (var reader = command.ExecuteReader())
                    {
                        // read through all rows
                        while (reader.Read())
                        {
                            String firstName = "";
                            String lastName = "";
                            int age = 0;
                            String address = "";
                            DateTime birthday = new DateTime();
                            double reputation = 2.0;
                            int pinsCompleted = 0;
                            String role = "";

                            int ordinal = reader.GetOrdinal("userID");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userID = reader.GetInt32(ordinal);
                            }

                            ordinal = reader.GetOrdinal("role");
                            if (!reader.IsDBNull(ordinal))
                            {
                                role = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("firstname");
                            if (!reader.IsDBNull(ordinal))
                            {
                                firstName = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("lastname");
                            if (!reader.IsDBNull(ordinal))
                            {
                                lastName = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("address");
                            if (!reader.IsDBNull(ordinal))
                            {
                                address = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("birthday");
                            if (!reader.IsDBNull(ordinal))
                            {
                                birthday = reader.GetDateTime(ordinal);
                            }
                            ordinal = reader.GetOrdinal("reputation");
                            if (!reader.IsDBNull(ordinal))
                            {
                                reputation = Decimal.ToDouble(reader.GetDecimal(ordinal));
                            }
                            ordinal = reader.GetOrdinal("tally");
                            if (!reader.IsDBNull (ordinal))
                            {
                                pinsCompleted = reader.GetInt32(ordinal);
                            }
                            userProfile = new UserProfile(userID, firstName, lastName, address, birthday, reputation, pinsCompleted, new GenericIdentity(userID.ToString(), role));
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message + $", {{failed: {_connectionString}}}";
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message + $", {{failed: {_connectionString}}}";
                }
            }
            result.Data = userProfile;
            result.IsSuccessful = true;
            return result;
        }

        public async Task<DataResponse<List<RecoveryRequests>>> SelectRecoveryRequestsTable()
        {
            List<RecoveryRequests> resquestsList = new List<RecoveryRequests>(); 
            string sqlStatement = "SELECT * FROM dbo.RecoveryRequests INNER JOIN dbo.Users ON (dbo.RecoveryRequests.userID = dbo.Users.userID) WHERE fulfilled = 0 ORDER BY [TimeStamp] asc";
            using (var connect = new SqlConnection(_connectionString))
            {
                int userId = 0;
                int ordinal = 0;
                String username = "";
                String firstname = "", lastname = "", role = "";
                DateTime dateCreated = new DateTime(2000,1,1);
                try
                {
                    connect.Open();
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ordinal = reader.GetOrdinal("username");
                            if (!reader.IsDBNull(ordinal))
                            {
                                username = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("timestamp");
                            if (!reader.IsDBNull(ordinal))
                            {
                                dateCreated = reader.GetDateTime(ordinal);
                            }

                            /*if (!reader.IsDBNull(0))
                            {
                                userId = reader.GetInt32(0);
                            }
                            ordinal = reader.GetOrdinal("firstName");
                            if (!reader.IsDBNull(ordinal))
                            {
                                firstname = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("lastName");
                            if (!reader.IsDBNull(ordinal))
                            {
                                lastname = reader.GetString(ordinal);
                            }
                            //sometimes throws exceptions with blank message
                            //if so change ordinal to 9
                            //ordinal = reader.GetOrdinal("role");
                            if (!reader.IsDBNull(9))
                            {
                                role = reader.GetString(9);
                            }
                            ordinal = reader.GetOrdinal("birthday");
                            if (!reader.IsDBNull(ordinal))
                            {
                                birthday = reader.GetDateTime(ordinal);
                            }*/
                            //UserProfile profile = new UserProfile(userId, firstname, lastname, "", birthday, new GenericIdentity(role));
                            //requests.Add(profile);
                            resquestsList.Add(new RecoveryRequests(username, dateCreated.ToString()));
                        }
                        reader.Close();
                    }
                    connect.Close();
                    return new DataResponse<List<RecoveryRequests>>(true, "Sql command passed", resquestsList);
                }
                catch (SqlException s)
                {
                    return new DataResponse<List<RecoveryRequests>>(false, s.Message + ", {failed: ExecuteReader}");
                    //Console.WriteLine(s.StackTrace);
                }
                catch (IndexOutOfRangeException ie)
                {
                    return new DataResponse<List<RecoveryRequests>>(false, ie.Message + ", {failed: ExecuteReader}");
                    //Console.WriteLine(ie.Message);
                }
                catch (Exception e)
                {
                    return new DataResponse<List<RecoveryRequests>>(false, e.Message + ", {failed: ExecuteReader}");
                    //Console.WriteLine(e.StackTrace);
                }
            }
        }

        public async Task<DataResponse<ValidRecovery>> SelectRecoveryUser(int userID)
        {
            DataResponse<ValidRecovery> validRecovery = new DataResponse<ValidRecovery>();
            string sqlStatement = "Select TOP 1 digest, salt FROM dbo.RecoveryRequests WHERE fulfilled = 0 AND userID = @ID Order by [timestamp] desc";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    var command = new SqlCommand(sqlStatement, connect);
                    command.Parameters.Add(new SqlParameter("@ID", userID));
                    String password = "";
                    String salt = "";
                    using (var reader = command.ExecuteReader())
                    {
                        // read through all rows
                        while (reader.Read())
                        {
                            int ordinal = reader.GetOrdinal("digest");
                            if (!reader.IsDBNull(ordinal))
                            {
                                password = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("salt");
                            if (!reader.IsDBNull(ordinal))
                            {
                                salt = reader.GetString(ordinal);
                            }
                            validRecovery.Data = new ValidRecovery(password, salt);
                            break;
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    validRecovery.IsSuccessful = false;
                    validRecovery.ErrorMessage = s.Message + $", {{failed: {_connectionString}}}";
                    return validRecovery;
                }
                catch (Exception e)
                {
                    validRecovery.IsSuccessful = false;
                    validRecovery.ErrorMessage = e.Message + $", {{failed: {_connectionString}}}";
                    return validRecovery;
                }
            }

            validRecovery.IsSuccessful = true;
            return validRecovery;


            /*using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string sqlSelect = "Select Top 1 newpassword, [timestamp] from dbo.RecoveryRequests WHERE fulfilled = 0 AND userId = @ID Order by [timestamp] desc; ";
                try
                {
                    var command = new SqlCommand(sqlSelect, connect);
                    command.Parameters.Add(new SqlParameter("@ID", userID));
                    String newPassword = (String)command.ExecuteScalar();
                    if (newPassword != null && newPassword != "")
                    {
                        password.data = newPassword;
                        password.isSuccessful = true;
                    }
                    else
                    {
                        password.isSuccessful = false;
                        password.errorMessage = "No Requests Found from User";
                    }
                }
                catch (SqlException s)
                {
                    password.isSuccessful = false;
                    password.errorMessage = s.Message + ", {failed: ExecuteScalar}";
                }
                catch (Exception e)
                {
                    password.isSuccessful = false;
                    password.errorMessage = e.Message + ", {failed: ExecuteScalar}";
                }
            }
            return password;
            */

        }

        // TODO: Change SelectEventCount to return DataResponse with the proper datatype for the response
        public async Task<DataResponse<String>> SelectUserProfileRole(int userID)
        {
            string sqlStatement = "SELECT role FROM dbo.UserProfiles WHERE userID = @userID";
            // var connection = new SqlConnection(_connectionString);
            var response = new DataResponse<String>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string role = reader.GetString(reader.GetOrdinal("role"));
                            response.Data = role;
                            response.IsSuccessful = true;
                        }
                    }
                    else
                    {
                        response.ErrorMessage = "No role associated with user ID";

                    }
                }
            }


            return response;
        }

        // TODO: Change SelectEventCount to return DataResponse with the proper datatype for the response
        public async Task<DataResponse<String>> SelectUserHash(int userID)
        {
            var sqlstatement = "SELECT userHash FROM dbo.Users WHERE userID = @userID";
            var response = new DataResponse<String>();
            using (var connection = new SqlConnection(_connectionString))
            {
                // Open the connection async
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                // Sql to return the userHash associated with the passed in userID 
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            string userHash = reader.GetString(reader.GetOrdinal("userHash"));

                            // Response obj stores the userHash value inside of the data property
                            response.Data = userHash;
                            response.IsSuccessful = true;

                        }
                    }
                    else
                    {
                        response.ErrorMessage = "Error getting User Hash";

                    }
                }
            }

            return response;
        }

        // TODO: Change SelectEventCount to return DataResponse with the proper datatype for the response
        public async Task<DataResponse<int>> SelectUserID(string email)
        {
            var sqlstatement = "SELECT userID FROM dbo.Users WHERE username = @email";
            var response = new DataResponse<int>();
            using (var connection = new SqlConnection(_connectionString))
            {
                // Open the connection async
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                // Sql to return the userHash associated with the passed in userID 
                cmd.Parameters.AddWithValue("@email", email);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            int userID = reader.GetInt32(reader.GetOrdinal("userID"));

                            // Response obj stores the userHash value inside of the data property
                            response.Data = userID;
                            response.IsSuccessful = true;

                        }
                    }
                    else
                    {
                        response.ErrorMessage = "Error getting User Hash";

                    }
                }
            }

            return response;

        }



        //------------------------------------------------------------------------
        // IUsersDBUpdater
        //------------------------------------------------------------------------

        public async Task<Response> UpdateRecoveryFulfilled(int userID)
        {
            var sql = "UPDATE dbo.RecoveryRequests SET fulfilled = 1 WHERE userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var response = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (response.ErrorMessage.Equals("Nothing Affected"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "No Request for User Found";
            }
            else if (response.IsSuccessful)
            {
                response.IsSuccessful = true;
                response.ErrorMessage = "Account recovery completed successfully for user";
            }
            return response;
        }

        public async Task<Response> UpdateUserPassword(int userID, String password, String salt)
        {
            var sql = "UPDATE dbo.Users SET password = @pass, salt = @salt, \"disabled\" = 0 WHERE userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            command.Parameters.Add(new SqlParameter("@pass", password));
            command.Parameters.Add(new SqlParameter("@salt", salt));
            var response = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (response.ErrorMessage.Equals("Nothing Affected"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "No Request for User Found";
            }
            else if (response.IsSuccessful)
            {
                response.IsSuccessful = true;
                response.ErrorMessage = "Account recovery completed successfully for user";
            }
            return response;
        }

        public async Task<Response> UpdatePinCompletionTallyAsync(int userID, int completionTally)
        {
            Response result = new Response();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        await connection.OpenAsync().ConfigureAwait(false);

                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = Convert.ToString(_configuration["Reputation:StoredProcedures:UpdateTally:Name"]);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue(Convert.ToString(_configuration["Reputation:StoredProcedures:UpdateTally:Parameter1"]), userID);
                            command.Parameters.AddWithValue(Convert.ToString(_configuration["Reputation:StoredProcedures:UpdateTally:Parameter2"]), completionTally + 1);

                            int updateToll = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                            if (updateToll > 0)
                            {
                                result.IsSuccessful = true;
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        result.ErrorMessage = e.Message;
                    }
                }
            }
            catch (SqlException ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        public async Task<Response> UpdateUserRoleAsync(UserProfile userProfile)
        {
            Response result = new Response();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        await connection.OpenAsync().ConfigureAwait(false);

                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = Convert.ToString(_configuration["Reputation:StoredProcedures:ChangeRole:Name"]);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue(Convert.ToString(_configuration["Reputation:StoredProcedures:ChangeRole:Parameter1"]), userProfile.UserID);
                            command.Parameters.AddWithValue(Convert.ToString(_configuration["Reputation:StoredProcedures:ChangeRole:Parameter2"]), userProfile.Identity.AuthenticationType);

                            int updateRole = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                            if (updateRole == 1)
                            {
                                result.IsSuccessful = true;
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        result.ErrorMessage = e.Message;
                    }
                }
            }
            catch (SqlException e)
            {
                result.ErrorMessage = e.Message;
            }

            return result;
        }

        public async Task<Response> UpdateUserReputationAsync(int user, double newReputation)
        {
            Response result = new Response();
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            CancellationToken token = cts.Token;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = Convert.ToString(_configuration["Reputation:StoredProcedures:ChangeReputation:Name"]);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(Convert.ToString(_configuration["Reputation:StoredProcedures:ChangeReputation:Parameter1"]), user);
                        command.Parameters.AddWithValue(Convert.ToString(_configuration["Reputation:StoredProcedures:ChangeReputation:Parameter2"]), newReputation);

                        int execute = await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);

                        if (execute == 1)
                        {
                            result.IsSuccessful = true;
                        }
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
            }
            return result;
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<int>> UpdateServiceRole(int userid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new DataResponse<int>();

                var insertSql = "UpdateRoleService";
                var command = new SqlCommand(insertSql, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@ID", userid));

                try
                {
                    result.Data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Microsoft.Data.SqlClient.SqlException e)
                {
                    result.ErrorMessage = e.ToString();
                    result.IsSuccessful = false;
                    result.Data = 0;
                }
                return result;
            }

        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<int>> UpdateRemoveServiceRole(int userid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = new DataResponse<int>();

                var insertSql = "UpdateRemoveServiceRole";
                var command = new SqlCommand(insertSql, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@ID", userid));

                try
                {
                    result.Data = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Microsoft.Data.SqlClient.SqlException e)
                {
                    result.ErrorMessage = e.ToString();
                    result.IsSuccessful = false;
                    result.Data = 0;
                }
                return result;
            }
        }


        //------------------------------------------------------------------------
        // IUsersDBDeleter
        //------------------------------------------------------------------------

        public async Task<Response> DeletePIIUserProfile(int userID)
        {
            var sql = "DELETE FROM dbo.UserProfiles WHERE userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var response = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (response.ErrorMessage.Equals("Nothing Affected"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Could not delete User Profile.";
            }
            else if (response.IsSuccessful)
            {
                response.IsSuccessful = true;
                response.ErrorMessage = "User Profile successfully deleted.";
            }

            return response;
        }

        public async Task<Response> DeletePIIUserAccount(int userID)
        {
            var sql = "DELETE FROM dbo.Users WHERE userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var response = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (response.ErrorMessage.Equals("Nothing Affected"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Could not delete User Account.";
            }
            else if (response.IsSuccessful)
            {
                response.IsSuccessful = true;
                response.ErrorMessage = "User Account successfully deleted.";
            }

            return response;
        }

        
    }
}

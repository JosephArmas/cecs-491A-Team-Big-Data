using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;


namespace TeamBigData.Utification.SQLDataAccess.UsersDB
{
    public class UsersSqlDAO : DbContext, IUsersDBInserter, IUsersDBSelecter, IUsersDBUpdater
    {
        private readonly String _connectionString;

        public UsersSqlDAO(DbContextOptions<UsersSqlDAO> options) : base(options) 
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
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
                    result.isSuccessful = true;
                    result.errorMessage = "SqlCommand Passed";
                }
                else if (rows == 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "Nothing Affected";
                }
                connection.Close();
            }
            catch (SqlException s)
            {
                result.errorMessage = s.Message + ", {failed: command.ExecuteNonQuery}";
            }
            catch (Exception e)
            {
                result.errorMessage = e.Message + ", {failed: command.ExecuteNonQuery}";
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
            if (!result.isSuccessful)
            {
                result.errorMessage += ", {failed: ExecuteSqlCommand}";
            }
            else
            {
                result.isSuccessful = true;
            }
            return result;
        }

        public async Task<Response> InsertUserProfile(int userId)
        {
            //Creates an Insert SQL statements using the collumn names and values given
            var insertSql = "INSERT into dbo.UserProfiles(userID, firstname, lastname, \"address\", birthday, \"role\") values" +
                "(@uID, @n, @ln, @add, @bday, @role)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@uID", userId));
            command.Parameters.Add(new SqlParameter("@n", ""));
            command.Parameters.Add(new SqlParameter("@ln", ""));
            command.Parameters.Add(new SqlParameter("@add", ""));
            command.Parameters.Add(new SqlParameter("@bday", (new DateTime(2000, 1, 1)).ToString()));
            command.Parameters.Add(new SqlParameter("@role", "Regular User"));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: ExecuteSqlCommand}";
            }
            else
            {
                result.isSuccessful = true;
            }
            return result;
        }

        public async Task<Response> InsertRecoveryRequest(int userID, String password, String salt)
        {
            String insertSql = "Insert into dbo.RecoveryRequests(userID, newPassword, salt) values (@ID, @newP, @salt)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            command.Parameters.Add(new SqlParameter("@newP", password));
            command.Parameters.Add(new SqlParameter("@salt", salt));
            var response = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (response.errorMessage.Contains("conflicted with the FOREIGN KEY constraint \"RR_ForeignKey_01\""))
            {
                response.isSuccessful = false;
                response.errorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
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
                    connect.Open();
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
                            userAccount.data = new UserAccount(userID, userName, password, salt, userHash, verified);
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    userAccount.errorMessage = s.Message + ", {failed: ExecuteReader}";
                }
                catch (Exception e)
                {
                    userAccount.errorMessage = e.Message + ", {failed: ExecuteReader}";
                }
            }
            if (userAccount.data == null)
            {
                userAccount.isSuccessful = false;
                userAccount.errorMessage += ", {failed: userAccount null}";
                return userAccount;
            }
            else if (userAccount.data._userID > 1000)
            {
                userAccount.isSuccessful = true;
                userAccount.errorMessage = "UserAccount Found";
                return userAccount;
            }
            else
            {
                userAccount.isSuccessful = false;
                userAccount.errorMessage = "No UserAccount Found";
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
                    connect.Open();
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
                            userProfile = new UserProfile(userID, firstName, lastName, address, birthday, new GenericIdentity(userID.ToString(), role));
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message + $", {{failed: {_connectionString}}}";
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message + $", {{failed: {_connectionString}}}";
                }
            }
            result.data = userProfile;
            result.isSuccessful = true;
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
            string sqlStatement = "Select TOP 1 newPassword, salt FROM dbo.RecoveryRequests WHERE fulfilled = 0 AND userID = @ID Order by [timestamp] desc";
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
                            int ordinal = reader.GetOrdinal("newPassword");
                            if (!reader.IsDBNull(ordinal))
                            {
                                password = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("salt");
                            if (!reader.IsDBNull(ordinal))
                            {
                                salt = reader.GetString(ordinal);
                            }
                            validRecovery.data = new ValidRecovery(password, salt);
                            break;
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    validRecovery.isSuccessful = false;
                    validRecovery.errorMessage = s.Message + $", {{failed: {_connectionString}}}";
                    return validRecovery;
                }
                catch (Exception e)
                {
                    validRecovery.isSuccessful = false;
                    validRecovery.errorMessage = e.Message + $", {{failed: {_connectionString}}}";
                    return validRecovery;
                }
            }

            validRecovery.isSuccessful = true;
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
            if (response.errorMessage.Equals("Nothing Affected"))
            {
                response.isSuccessful = false;
                response.errorMessage = "No Request for User Found";
            }
            else if (response.isSuccessful)
            {
                response.isSuccessful = true;
                response.errorMessage = "Account recovery completed successfully for user";
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
            if (response.errorMessage.Equals("Nothing Affected"))
            {
                response.isSuccessful = false;
                response.errorMessage = "No Request for User Found";
            }
            else if (response.isSuccessful)
            {
                response.isSuccessful = true;
                response.errorMessage = "Account recovery completed successfully for user";
            }
            return response;
        }
    }
}

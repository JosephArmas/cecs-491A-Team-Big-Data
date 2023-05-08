using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Collections;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SqlDAO : DbContext, IDBAnalysis, IDAO //, IDBInserter, IDBCounter, IDBSelecter, IDBUpdater,
    {
        private readonly String _connectionString;

        public SqlDAO(DbContextOptions<SqlDAO> options) : base(options)
        {

            _connectionString = this.Database.GetDbConnection().ConnectionString;
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
                //result.Data = rows;
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

        public Task<Response> Execute(String sql)
        {
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            return ExecuteSqlCommand(connection, command);
        }

        //----------------------------------------------------------------------------------------
        // IDBInserter
        //----------------------------------------------------------------------------------------
        public async Task<Response> InsertUser(String email, String digest, String salt, String userhash)
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

        public async Task<Response> InsertUserHash(String userHash, int userID)
        {
            var insertSql = "INSERT into dbo.UserHash(userHash, \"userID\") values(@hash, @ID)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@hash", userHash));
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var result = await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.ErrorMessage += $", {{failed: ExecuteSqlCommand, connectionstring:{_connectionString} }}";
            }
            else
            {
                result.IsSuccessful = true;
            }
            return result;
        }

        public async Task<Response> IncrementUserAccountDisabled(UserAccount userAccount)
        {
            var updateSql = "UPDATE dbo.Users set \"disabled\" += 1 Where userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(updateSql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userAccount.UserID));
            return await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
        }
        
        /*
        public async Task<Response> InsertPin(Pin pin)
        {
            var sql = "INSERT INTO dbo.Pins (userID,lat,lng,pinType,\"description\",\"disabled\",completed,\"dateTime\")" +
                "values(@ID, @lat, @lng, @type, @d, @dis, @c, @dt)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", pin.UserID));
            command.Parameters.Add(new SqlParameter("@lat", pin.Lat));
            command.Parameters.Add(new SqlParameter("@lng", pin.Lng));
            command.Parameters.Add(new SqlParameter("@type", pin.PinType));
            command.Parameters.Add(new SqlParameter("@d", pin.Description));
            command.Parameters.Add(new SqlParameter("@dis", pin.Disabled));
            command.Parameters.Add(new SqlParameter("@c", pin.C));
            command.Parameters.Add(new SqlParameter("@dt", pin.Da));
            return await ExecuteSqlCommand(connection, command).ConfigureAwait(false);
        }
        */

        //----------------------------------------------------------------------------------------
        // IDBCounter
        //----------------------------------------------------------------------------------------
        public Task<Response> CountSalt(String salt)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Creates an Insert SQL statements using the collumn names and values given
                var countSql = "SELECT COUNT (salt) FROM dbo.Users Where salt = @s";
                try
                {
                    var command = new SqlCommand(countSql, connection);
                    command.Parameters.Add(new SqlParameter("@s", salt));
                    result.Data = command.ExecuteScalar();
                    result.IsSuccessful = true;
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<Response> CountUserLoginAttempts(int userId)
        {
            var tcs = new TaskCompletionSource<Response>();
            var list = new ArrayList();
            Response result = new Response();
            result.IsSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select LogLevel from dbo.Logs Where \"user\" = @ID AND " +
                    "\"timestamp\" >= DATEADD(day, -1, getDate()) order by \"timestamp\" asc";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    command.Parameters.Add(new SqlParameter("@ID", userId));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                    result.IsSuccessful = true;
                    result.Data = list;
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
            }
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<Response> CountLogs()
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Creates an Insert SQL statements using the collumn names and values given
                var countSql = "SELECT COUNT (logID) FROM dbo.Logs";
                try
                {
                    var command = new SqlCommand(countSql, connection);
                    result.Data = (int)command.ExecuteScalar();
                    result.IsSuccessful = true;
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }


        //----------------------------------------------------------------------------------------
        // IDBSelecter
        //----------------------------------------------------------------------------------------
        public async Task<DataResponse<List<UserProfile>>> SelectUserProfileTable(String roleName)
        {
            var tcs = new TaskCompletionSource<DataResponse<List<UserProfile>>>();
            var userProfiles = new List<UserProfile>();
            var result = new DataResponse<List<UserProfile>>();
            if (!roleName.Equals("Admin User"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Authorizatino";
                tcs.SetResult(result);
                return result;
            }
            string sqlStatement = "SELECT * FROM dbo.UserProfile";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                    {
                        // read through all rows
                        while (reader.Read())
                        {
                            int userID = 0;
                            String firstName = "";
                            String lastName = "";
                            int age = 0;
                            String email = "";
                            String address = "";
                            DateTime birthday = new DateTime();
                            String role = "";
                            double reputation = 0.0;
                            int ordinal = reader.GetOrdinal("userID");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userID = reader.GetInt32(ordinal);
                            }

                            ordinal = reader.GetOrdinal("Role");
                            if (!reader.IsDBNull(ordinal))
                            {
                                role = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("FirstName");
                            if (!reader.IsDBNull(ordinal))
                            {
                                firstName = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("LastName");
                            if (!reader.IsDBNull(ordinal))
                            {
                                lastName = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("Address");
                            if (!reader.IsDBNull(ordinal))
                            {
                                address = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("Birthday");
                            if (!reader.IsDBNull(ordinal))
                            {
                                birthday = reader.GetDateTime(ordinal);
                            }
                            ordinal = reader.GetOrdinal("reputation");
                            if (!reader.IsDBNull(ordinal))
                            {
                                reputation = (double)reader.GetDecimal(ordinal);
                            }
                            userProfiles.Add(new UserProfile(userID, firstName, lastName, address, birthday, reputation, new GenericIdentity(userID.ToString(), role)));
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                if (userProfiles.Count > 0)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "Returning List of UserProfiles";
                }
                else if (userProfiles.Count == 0 && result.ErrorMessage.Equals(""))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Empty List of UserProfiles";
                }
            }
            result.Data = userProfiles;
            tcs.SetResult(result);
            return result;
        }
        public Task<DataResponse<UserProfile>> SelectUserProfile(int userID)
        {
            var tcs = new TaskCompletionSource<DataResponse<UserProfile>>();
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
                            userID = 0;
                            String firstName = "";
                            String lastName = "";
                            int age = 0;
                            String address = "";
                            DateTime birthday = new DateTime();
                            String role = "";
                            double reputation = 0.0;

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
                                reputation = (double)reader.GetDecimal(ordinal);
                            }
                            userProfile = new UserProfile(userID, firstName, lastName, address, birthday, reputation, new GenericIdentity(userID.ToString(), role));
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
            }
            result.Data = userProfile;
            tcs.SetResult(result);
            return tcs.Task;
        }

        public async Task<DataResponse<UserAccount>> SelectUserAccount(String username)
        {
            var tcs = new TaskCompletionSource<DataResponse<UserAccount>>();
            var result = new DataResponse<UserAccount>();
            var userAccount = new UserAccount();
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
                            userAccount = new UserAccount(userID, userName, password, salt, userHash, verified);
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
            }
            if (userAccount.UserID > 0)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "UserAccount Found";
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No UserAccount Found";
            }
            result.Data = userAccount;
            tcs.SetResult(result);
            return result;
        }
        public Task<DataResponse<List<UserAccount>>> SelectUserAccountTable(String role)
        {
            var tcs = new TaskCompletionSource<DataResponse<List<UserAccount>>>();
            var result = new DataResponse<List<UserAccount>>();
            var userAccounts = new List<UserAccount>();
            if (role != "Admin User")
            {
                result.ErrorMessage = "Invalid Authorization";
                result.IsSuccessful = false;
                tcs.SetResult(result);
                return tcs.Task;
            }
            string sqlStatement = "SELECT * FROM dbo.Users";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
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
                                if (reader.GetInt32(ordinal) > 0)
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
                            userAccounts.Add(new UserAccount(userID, userName, password, salt, userHash, verified));
                        }
                        reader.Close();
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                if (userAccounts.Count > 0)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "Returning List of UserAccounts";
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Empty List of UserAccounts";
                }
            }
            result.Data = userAccounts;
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<Response> SelectLastUserID()
        {
            var tcs = new TaskCompletionSource<Response>();
            Response response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "SELECT TOP(1) userID FROM dbo.Users ORDER BY userID DESC";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    var rows = command.ExecuteReader();
                    while (rows.Read())
                    {
                        response.Data = rows.GetInt32(0);
                        response.IsSuccessful = true;
                    }
                }
                catch (SqlException s)
                {
                    response.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    response.ErrorMessage = e.Message;
                }
            }
            tcs.SetResult(response);
            return tcs.Task;
        }

        //IDB Recovery
        public Task<DataResponse<List<UserProfile>>> GetRecoveryRequests()
        {
            var response = new DataResponse<List<UserProfile>>();
            var tcs = new TaskCompletionSource<DataResponse<List<UserProfile>>>();
            var requests = new List<UserProfile>();
            string sqlStatement = "SELECT * FROM dbo.RecoveryRequests join dbo.UserProfiles on(dbo.RecoveryRequests.userId = dbo.UserProfiles.userId) WHERE fulfilled = 0 ORDER BY [TimeStamp] asc";
            using (var connect = new SqlConnection(_connectionString))
            {
                int userId = 0;
                int ordinal = 0;
                double reputation = 0.0;
                String firstname = "", lastname = "", role = "";
                DateTime birthday = new DateTime();
                try
                {
                    connect.Open();
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
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
                            ordinal = reader.GetOrdinal("role");
                            if (!reader.IsDBNull(ordinal))
                            {
                                role = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("birthday");
                            if (!reader.IsDBNull(ordinal))
                            {
                                birthday = reader.GetDateTime(ordinal);
                            }
                            ordinal = reader.GetOrdinal("reputation");
                            if(!reader.IsDBNull(ordinal))
                            {
                                reputation = (double)reader.GetDecimal(ordinal);
                            }
                            UserProfile profile = new UserProfile(userId, firstname, lastname, "", birthday, reputation, new GenericIdentity(role));
                            requests.Add(profile);
                        }
                        reader.Close();
                        response.IsSuccessful = true;
                        response.Data = requests;
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    response.ErrorMessage = s.Message;
                    Console.WriteLine(s.StackTrace);
                }
                catch (IndexOutOfRangeException ie)
                {
                    response.ErrorMessage = ie.Message;
                    Console.WriteLine(ie.Message);
                }
                catch (Exception e)
                {
                    response.ErrorMessage = e.Message;
                    Console.WriteLine(e.StackTrace);
                }
            }
            tcs.SetResult(response);
            return tcs.Task;
        }

        public Task<Response> GetNewPassword(int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
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
                        result.Data = newPassword;
                        result.IsSuccessful = true;
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "No Requests Found from User";
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
            }
            tcs.SetResult(result);
            return tcs.Task;
        }

        public async Task<Response> ResetAccount(int userID, String newPassword)
        {
            var updateSql = "UPDATE dbo.Users set password = @newP, \"disabled\" = 0 where userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(updateSql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            command.Parameters.Add(new SqlParameter("@newP", newPassword));
            var response = await ExecuteSqlCommand(connection, command);
            if (response.ErrorMessage.Equals("Nothing Affected"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
            }
            return response;
        }

        public async Task<Response> RequestFulfilled(int userID)
        {
            var sql = "UPDATE dbo.RecoveryRequests SET fulfilled = 1 WHERE userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            var response = await ExecuteSqlCommand(connection, command);
            if (response.ErrorMessage.Equals("Nothing Affected"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "No Request for User Found";
            }
            else if (response.IsSuccessful)
            {
                response.ErrorMessage = "Account recovery completed successfully for user";
            }
            return response;
        }


        //----------------------------------------------------------------------------------------
        // IDBUpdater
        //----------------------------------------------------------------------------------------
        public Task<Response> UpdateUserProfile(UserProfile user)
        {
            var updateSql = "UPDATE dbo.UserProfiles set firstname = @n, lastname = @ln, \"address\" = @add, birthday = @bday, userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(updateSql, connection);
            command.Parameters.Add(new SqlParameter("@n", user.FirstName));
            command.Parameters.Add(new SqlParameter("@ln", user.LastName));
            command.Parameters.Add(new SqlParameter("@add", user.Address));
            command.Parameters.Add(new SqlParameter("@bday", user.Birthday));
            command.Parameters.Add(new SqlParameter("@ID", user.UserID));
            return ExecuteSqlCommand(connection, command);
        }
        public async Task<Response> UpdatePinToComplete(int pinID)
        {
            var sql = "UPDATE dbo.Pins SET completed = 1 WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            var result = await ExecuteSqlCommand(connection, command);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.ErrorMessage = "Update Pin To Complete successfully for user";
            }
            return result;
        }

        public async Task<Response> UpdatePinType(int pinID, int pinType)
        {
            var sql = "UPDATE dbo.Pins SET pinType = @t WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@t", pinType));
            var result = await ExecuteSqlCommand(connection, command);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.ErrorMessage = "Update Pin Type successfully for user";
            }
            return result;
        }

        public async Task<Response> UpdatePinContent(int pinID, string description)
        {
            var sql = "UPDATE dbo.Pins SET \"description\" = @d WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            command.Parameters.Add(new SqlParameter("@d", description));
            var result = await ExecuteSqlCommand(connection, command);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.ErrorMessage = "Update Pin Content successfully for user";
            }
            return result;
        }

        public async Task<Response> UpdatePinToDisabled(int pinID)
        {
            var sql = "UPDATE dbo.Pins SET \"disabled\" = 1 WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            var result = await ExecuteSqlCommand(connection, command);
            if (result.ErrorMessage.Equals("Nothing Affected"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "No Request for Pin Found";
            }
            else if (result.IsSuccessful)
            {
                result.ErrorMessage = "Update Pin To Disabled successfully for user";
            }
            return result;
        }

        //----------------------------------------------------------------------------------------
        // IDBAnalysis
        //----------------------------------------------------------------------------------------
        public Task<DataResponse<int[]>> GetNewLogins()
        {
            var tcs = new TaskCompletionSource<DataResponse<int[]>>();
            var result = new DataResponse<int[]>();
            var rows = new int[91];
            result.IsSuccessful = false;
            int daysAgo, logins, i;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select * from dbo.loginsPast3Months order by DaysAgo";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.IsSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Logins");
                        logins = reader.GetInt32(i);
                        rows[90 - daysAgo] = logins;
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                result.Data = rows;
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<DataResponse<int[]>> GetNewRegistrations()
        {
            var tcs = new TaskCompletionSource<DataResponse<int[]>>();
            var result = new DataResponse<int[]>();
            var rows = new int[91];
            result.IsSuccessful = false;
            int daysAgo, registrations, i;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select * from dbo.registrationsPast3Months order by DaysAgo";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.IsSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Registrations");
                        registrations = reader.GetInt32(i);
                        rows[90 - daysAgo] = registrations;
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }

                result.Data = rows;
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<DataResponse<int[]>> GetPinsAdded()
        {
            var tcs = new TaskCompletionSource<DataResponse<int[]>>();
            var result = new DataResponse<int[]>();
            var rows = new int[8];
            int daysAgo, pins, i;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select * from dbo.newPinsPastWeek order by DaysAgo";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.IsSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Pins");
                        pins = reader.GetInt32(i);
                        rows[7 - daysAgo] = pins;
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }

                result.Data = rows;
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<DataResponse<int[]>> GetPinPulls()
        {
            var tcs = new TaskCompletionSource<DataResponse<int[]>>();
            var result = new DataResponse<int[]>();
            var rows = new int[31];
            int daysAgo, pinPulls, i;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select * from dbo.pinPullsPastMonth order by DaysAgo";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.IsSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Pins");
                        pinPulls = reader.GetInt32(i);
                        rows[30 - daysAgo] = pinPulls;
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }

                result.Data = rows;
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        //----------------------------------------------------------------------------------------
        // IDBDeleter
        //----------------------------------------------------------------------------------------
        public Task<Response> DeleteUser(string username)
        {
            var deleteSql1 = "DELETE FROM dbo.UserProfiles WHERE usernamw = @u";
            var deleteSql2 = "DELETE FROM dbo.Users WHERE username = @u";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(deleteSql1, connection);
            var command2 = new SqlCommand(deleteSql2, connection);
            command.Parameters.Add(new SqlParameter("@u", username));
            command2.Parameters.Add(new SqlParameter("@u", username));
            ExecuteSqlCommand(connection, command);
            return ExecuteSqlCommand(connection, command2);
        }




        //----------------------------------------------------------------------------------------
        // Other
        //----------------------------------------------------------------------------------------
        public Task<Response> ChangePassword(String username, String newPassword)
        {
            var updateSql = "UPDATE dbo.Users set password = @p where username = @u";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(updateSql, connection);
            command.Parameters.Add(new SqlParameter("@u", username));
            command.Parameters.Add(new SqlParameter("@p", newPassword));
            return ExecuteSqlCommand(connection, command);
        }

        public Task<Response> DeleteUserProfile(int userID)
        {
            var deleteSql1 = "DELETE FROM dbo.UserProfiles WHERE userID = @ID";
            var deleteSql2 = "DELETE FROM dbo.Users WHERE userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(deleteSql1, connection);
            var command2 = new SqlCommand(deleteSql2, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            command2.Parameters.Add(new SqlParameter("@ID", userID));
            ExecuteSqlCommand(connection, command);
            return ExecuteSqlCommand(connection, command2);
        }


        public Task<DataResponse<UserProfile>> GetUser(UserAccount user)
        {
            var tcs = new TaskCompletionSource<DataResponse<UserProfile>>();
            var list = new Object[8];
            DataResponse<UserProfile> result = new DataResponse<UserProfile>();
            result.IsSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select dbo.users.userid, firstname, lastname, \"address\", birthday, role " +
                    "from dbo.Users left join dbo.UserProfiles on (dbo.Users.username = dbo.UserProfiles.username)" +
                    " Where dbo.Users.username = @u AND password = @p";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    command.Parameters.Add(new SqlParameter("@u", user.Username));
                    command.Parameters.Add(new SqlParameter("@p", user.Password));
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        result.IsSuccessful = true;
                        reader.GetValues(list);
                        var userProfile = new UserProfile((int)list[0], (string)list[1], (string)list[2], (string)list[3],
                            ((DateTime)list[4]), ((double)list[5]), new GenericIdentity((string)list[5]));
                        result.Data = userProfile;
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Error: Invalid Username or Password";
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<Response> GetLoginAttempts(String username)
        {
            var tcs = new TaskCompletionSource<Response>();
            var list = new ArrayList();
            Response result = new Response();
            result.IsSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select LogLevel from dbo.Logs Where \"user\" = @u AND " +
                    "\"timestamp\" >= DATEADD(day, -1, getDate()) order by \"timestamp\" asc";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    command.Parameters.Add(new SqlParameter("@u", username));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                    result.IsSuccessful = true;
                    result.Data = list;
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<bool> IsValidUsername(String username)
        {
            var tcs = new TaskCompletionSource<bool>();
            String sqlStatement = "Select COUNT(Username) FROM dbo.Users WHERE Username = @u";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    var command = new SqlCommand(sqlStatement, connect);
                    command.Parameters.Add(new SqlParameter("@u", username));
                    if ((int)command.ExecuteScalar() == 1)
                    {
                        tcs.SetResult(true);
                    }
                    else
                    {
                        tcs.SetResult(false);
                    }
                }
                catch (Exception e)
                {
                    tcs.SetResult(false);
                }
            }
            return tcs.Task;
        }

        public async Task<Response> CreateRecoveryRequest(int userID, String newPassword)
        {
            String insertSql = "Insert into dbo.RecoveryRequests(userID, newPassword) values (@ID, @newP)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userID));
            command.Parameters.Add(new SqlParameter("@newP", newPassword));
            var response = await ExecuteSqlCommand(connection, command);
            if (response.ErrorMessage.Contains("conflicted with the FOREIGN KEY constraint \"RR_ForeignKey_01\""))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
            }
            else if(response.IsSuccessful)
            {
                Console.WriteLine(response.ErrorMessage);
                response.ErrorMessage = "error";
            }
            return response;
        }
    }
}
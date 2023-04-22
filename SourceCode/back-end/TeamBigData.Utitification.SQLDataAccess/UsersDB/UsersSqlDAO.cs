using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
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

        public async Task<Response> InsertUserAccount(string email, string digest, string salt, string userhash)
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




        //------------------------------------------------------------------------
        // IUsersDBSelecter
        //------------------------------------------------------------------------

        public async Task<DataResponse<UserAccount>> SelectUserAccount(string username)
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
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
            }
            if (userAccount._userID > 0)
            {
                result.isSuccessful = true;
                result.errorMessage = "UserAccount Found";
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "No UserAccount Found";
            }
            result.data = userAccount;
            tcs.SetResult(result);
            return result;
        }

        public async Task<DataResponse<UserProfile>> SelectUserProfile(int userID)
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


        //------------------------------------------------------------------------
        // IUsersDBUpdater
        //------------------------------------------------------------------------


    }
}

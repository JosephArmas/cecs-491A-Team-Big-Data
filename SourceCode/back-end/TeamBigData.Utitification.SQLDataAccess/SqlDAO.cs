using System.Buffers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SqlDAO : IDBInserter, IDBCounter, IDAO, IDBSelecter, IDBUpdater, IDBAnalysis
    {
        private readonly String _connectionString;

        public SqlDAO(String connectionString)
        {
            _connectionString = connectionString;
        }

        private Task<Response> ExecuteSqlCommand(SqlConnection connection, SqlCommand command)
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
                }
                else if(rows == 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "Nothing Affected";
                }
                connection.Close();
            }
            catch (SqlException s)
            {
                result.errorMessage = s.Message;
            }
            catch (Exception e)
            {
                result.errorMessage = e.Message;
            }
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<Response> InsertUser(UserAccount user)
        {
            var insertSql = "INSERT INTO dbo.Users (username, \"password\", \"disabled\", salt, userHash) values(@u, @p, 0, @s, @h)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@u", user._username));
            command.Parameters.Add(new SqlParameter("@p", user._password));
            command.Parameters.Add(new SqlParameter("@s", user._salt));
            command.Parameters.Add(new SqlParameter("@h", user._userHash));
            return ExecuteSqlCommand(connection, command);
        }
        public Task<Response> InsertUserProfile(UserProfile user)
        {
            //Creates an Insert SQL statements using the collumn names and values given
            var insertSql = "INSERT into dbo.UserProfiles(userID, firstname, lastname, \"address\", birthday, \"role\") values" +
                "(@uID, @n, @ln, @add, @bday, @role)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@uID", user._userID));
            command.Parameters.Add(new SqlParameter("@n", user._firstName));
            command.Parameters.Add(new SqlParameter("@ln", user._lastName));
            command.Parameters.Add(new SqlParameter("@add", user._address));
            command.Parameters.Add(new SqlParameter("@bday", user._birthday));
            command.Parameters.Add(new SqlParameter("@role", user.Identity.AuthenticationType));
            return ExecuteSqlCommand(connection, command);
        }
        public Task<Response> InsertUserHash(String userHash,int userID)
        {
            var insertSql = "INSERT into dbo.UserHash(userHash, \"userID\") values(@hash, @ID)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(insertSql, connection);
            command.Parameters.Add(new SqlParameter("@hash", userHash));
            command.Parameters.Add(new SqlParameter("@ID", userID));
            return ExecuteSqlCommand(connection, command);
        }
        public Task<Response> IncrementUserAccountDisabled(UserAccount userAccount)
        {
            var updateSql = "UPDATE dbo.Users set \"disabled\" += 1 Where userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(updateSql, connection);
            command.Parameters.Add(new SqlParameter("@ID", userAccount._userID));
            return ExecuteSqlCommand(connection, command);
        }
        public  Task<Response> UpdateUserProfile(UserProfile user)
        {
            var updateSql = "UPDATE dbo.UserProfiles set firstname = @n, lastname = @ln, \"address\" = @add, birthday = @bday, userID = @ID";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(updateSql, connection);
            command.Parameters.Add(new SqlParameter("@n", user._firstName));
            command.Parameters.Add(new SqlParameter("@ln", user._lastName));
            command.Parameters.Add(new SqlParameter("@add", user._address));
            command.Parameters.Add(new SqlParameter("@bday", user._birthday));
            command.Parameters.Add(new SqlParameter("@ID", user._userID));
            return ExecuteSqlCommand(connection, command);
        }

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
        public Task<Response> DeleteUser(string username)
        {
            var deleteSql1 = "DELETE FROM dbo.UserProfiles WHERE username = @u";
            var deleteSql2 = "DELETE FROM dbo.Users WHERE username = @u";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(deleteSql1, connection);
            var command2 = new SqlCommand(deleteSql2, connection);
            command.Parameters.Add(new SqlParameter("@u", username));
            command2.Parameters.Add(new SqlParameter("@u", username));
            ExecuteSqlCommand(connection, command);
            return ExecuteSqlCommand(connection, command2);
        }

        public Task<Response> GetUser(UserAccount user)
        {
            var tcs = new TaskCompletionSource<Response>();
            var list = new Object[8];
            Response result = new Response();
            result.isSuccessful = false;
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
                    command.Parameters.Add(new SqlParameter("@u", user._username));
                    command.Parameters.Add(new SqlParameter("@p", user._password));
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        result.isSuccessful = true;
                        reader.GetValues(list);
                        var userProfile = new UserProfile((int)list[0], (string)list[1], (string)list[2], (string)list[3],
                            ((DateTime)list[4]), new GenericIdentity((string)list[5]));
                        result.data = userProfile;
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "Error: Invalid Username or Password";
                    }
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
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
            result.isSuccessful = false;
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
                    result.isSuccessful = true;
                    result.data = list;
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

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
                    result.data = command.ExecuteScalar();
                    result.isSuccessful = true; 
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
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
                    result.data = (int)command.ExecuteScalar();
                    result.isSuccessful = true;
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<DataResponse<List<UserProfile>>> SelectUserProfileTable(String roleName)
        {
            var tcs = new TaskCompletionSource<DataResponse<List<UserProfile>>>();
            var userProfiles = new List<UserProfile>();
            var result = new DataResponse<List<UserProfile>>();
            if (!roleName.Equals("Admin User"))
            {
                result.isSuccessful = false;
                result.errorMessage = "Invalid Authorizatino";
                tcs.SetResult(result);
                return tcs.Task;
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
                            userProfiles.Add(new UserProfile(userID, firstName, lastName, address, birthday, new GenericIdentity(userID.ToString(), role)));
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
                if (userProfiles.Count > 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "Returning List of UserProfiles";
                }
                else if(userProfiles.Count == 0 && result.errorMessage.Equals(""))
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Empty List of UserProfiles";
                }
            }
            result.data = userProfiles;
            tcs.SetResult(result);
            return tcs.Task;
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
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
            }
            result.data = userProfile;
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<Response> Execute(object req)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
            if (req.GetType() != typeof(string)) //Verifies if the parameter matches the acceptable string format type
            {
                result.errorMessage = "Error: input parameter for SqlDAO not of type string";
                result.isSuccessful = false;
                tcs.SetResult(result);
                return tcs.Task;
            }
            using (SqlConnection connect = new SqlConnection(_connectionString.ToString()))
            {
                connect.Open();
                try
                {
                    result.data = (new SqlCommand(req.ToString(), connect)).ExecuteNonQuery();
                    result.isSuccessful = true;
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
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
            if (response.errorMessage.Contains("conflicted with the FOREIGN KEY constraint \"RR_ForeignKey_01\""))
            {
                response.isSuccessful = false;
                response.errorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
            }
            return response;
        }
        public Task<DataResponse<UserAccount>> SelectUserAccount(String username)
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
            return tcs.Task;
        }
        public Task<DataResponse<List<UserAccount>>> SelectUserAccountTable(String role)
        {
            var tcs = new TaskCompletionSource<DataResponse<List<UserAccount>>>();
            var result = new DataResponse<List<UserAccount>>();
            var userAccounts = new List<UserAccount>();
            if (role != "Admin User") 
            {
                result.errorMessage = "Invalid Authorization";
                result.isSuccessful = false;
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
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                if (userAccounts.Count > 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "Returning List of UserAccounts";
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Empty List of UserAccounts";
                }
            }
            result.data = userAccounts;
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
                        response.data = rows.GetInt32(0);
                        response.isSuccessful = true;
                    }
                }
                catch (SqlException s)
                {
                    response.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    response.errorMessage = e.Message;
                }
            }
            tcs.SetResult(response);
            return tcs.Task;
        }

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
                            }
                            UserProfile profile = new UserProfile(userId, firstname, lastname, "", birthday, new GenericIdentity(role));
                            requests.Add(profile);
                        }
                        reader.Close();
                        response.isSuccessful = true;
                        response.data = requests;
                    }
                    connect.Close();
                }
                catch (SqlException s)
                {
                    response.errorMessage = s.Message;
                    Console.WriteLine(s.StackTrace);
                }
                catch (IndexOutOfRangeException ie)
                {
                    response.errorMessage = ie.Message;
                    Console.WriteLine(ie.Message);
                }
                catch (Exception e)
                {
                    response.errorMessage = e.Message;
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
                    if(newPassword != null && newPassword != "")
                    {
                        result.data = newPassword;
                        result.isSuccessful = true;
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "No Requests Found from User";
                    }
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
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<Response> CountUserLoginAttempts(int userId)
        {
            var tcs = new TaskCompletionSource<Response>();
            var list = new ArrayList();
            Response result = new Response();
            result.isSuccessful = false;
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
                    result.isSuccessful = true;
                    result.data = list;
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
            if (response.errorMessage.Equals("Nothing Affected"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
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
            if (response.errorMessage.Equals("Nothing Affected"))
            {
                response.isSuccessful = false;
                response.errorMessage = "No Request for User Found";
            }
            else if(response.isSuccessful)
            {
                response.errorMessage = "Account recovery completed successfully for user";
            }
            return response;
        }

        public Task<DataResponse<List<Pin>>> SelectPinTable()
        {
            var tcs = new TaskCompletionSource<DataResponse<List<Pin>>>();
            var result = new DataResponse<List<Pin>>();
            List<Pin> pins = new List<Pin>();
            string sqlStatement = "SELECT * FROM dbo.Pins";
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
                            int pinID = 0;
                            int userID = 0;
                            string lat = "";
                            string lng = "";
                            int pinType = 0;
                            string description = "";
                            int disabled = 0;
                            int completed = 0;
                            string dateTime = "";
                            pinID = reader.GetInt32(0);
                            userID = reader.GetInt32(1);
                            lat = reader.GetString(2);
                            lng = reader.GetString(3);
                            pinType = reader.GetInt32(4);
                            description = reader.GetString(5);
                            disabled = reader.GetInt32(6);
                            completed = reader.GetInt32(7);
                            dateTime = reader.GetString(8);
                            pins.Add(new Pin(pinID, userID, lat, lng, pinType, description, disabled, completed, dateTime));
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
                if (pins.Count > 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "Returning List of Pins";
                    result.data = pins;
                }
                else if (pins.Count == 0 && result.errorMessage.Equals(""))
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Empty List of Pins";
                    result.data = pins;
                }
            }
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<Response> InsertPin(Pin pin)
        {
            var sql = "INSERT INTO dbo.Pins (userID,lat,lng,pinType,\"description\",\"disabled\",completed,\"dateTime\")" +
                "values(@ID, @lat, @lng, @type, @d, @dis, @c, @dt)";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@ID", pin._userID));
            command.Parameters.Add(new SqlParameter("@lat", pin._lat));
            command.Parameters.Add(new SqlParameter("@lng", pin._lng));
            command.Parameters.Add(new SqlParameter("@type", pin._pinType));
            command.Parameters.Add(new SqlParameter("@d", pin._description));
            command.Parameters.Add(new SqlParameter("@dis", pin._disabled));
            command.Parameters.Add(new SqlParameter("@c", pin._completed));
            command.Parameters.Add(new SqlParameter("@dt", pin._dateTime));
            return ExecuteSqlCommand(connection, command);
        }

        public async Task<Response> UpdatePinToComplete(int pinID)
        {
            var sql = "UPDATE dbo.Pins SET completed = 1 WHERE pinID = @p";
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@p", pinID));
            var result = await ExecuteSqlCommand(connection, command);
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if(result.isSuccessful)
            {
                result.errorMessage = "Update Pin To Complete successfully for user";
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
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if(result.isSuccessful)
            {
                result.errorMessage = "Update Pin Type successfully for user";
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
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if (result.isSuccessful)
            {
                result.errorMessage = "Update Pin Content successfully for user";
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
            if (result.errorMessage.Equals("Nothing Affected"))
            {
                result.isSuccessful = false;
                result.errorMessage = "No Request for Pin Found";
            }
            else if (result.isSuccessful)
            {
                result.errorMessage = "Update Pin To Disabled successfully for user";
            }
            return result;
        }

        public Task<DataResponse<int[]>> GetNewLogins()
        {
            var tcs = new TaskCompletionSource<DataResponse<int[]>>();
            var result = new DataResponse<int[]>();
            var rows = new int[91];
            result.isSuccessful = false;
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
                        result.isSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Logins");
                        logins = reader.GetInt32(i);
                        rows[90 - daysAgo] = logins;
                    }
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                result.data = rows;
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<DataResponse<int[]>> GetNewRegistrations()
        {
            var tcs = new TaskCompletionSource<DataResponse<int[]>>();
            var result = new DataResponse<int[]>();
            var rows = new int[91];
            result.isSuccessful = false;
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
                        result.isSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Registrations");
                        registrations = reader.GetInt32(i);
                        rows[90 - daysAgo] = registrations;
                    }
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
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
                        result.isSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Pins");
                        pins = reader.GetInt32(i);
                        rows[7 - daysAgo] = pins;
                    }
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
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
                        result.isSuccessful = true;
                        i = reader.GetOrdinal("DaysAgo");
                        daysAgo = reader.GetInt32(i);
                        i = reader.GetOrdinal("Pins");
                        pinPulls = reader.GetInt32(i);
                        rows[30 - daysAgo] = pinPulls;
                    }
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
            
        }
        
        
        // Authorization checking / logging
        public Task<Response> UpdateUserRole(int userID, string role)
        {
            var sqlstatement = "UPDATE dbo.UserProfiles SET role = @r WHERE userID = @userID";
            var response = new Response();
            var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sqlstatement, connection);
            // Sql to return the userHash associated with the passed in userID 
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@r", role);
            return ExecuteSqlCommand(connection, cmd);

        }  
        public async Task<Response> SelectUserID(string email)
        {
            var sqlstatement = "SELECT userID FROM dbo.Users WHERE username = @email";
            var response = new Response();
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
                            response.data = userID;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting User Hash";

                    }
                }
            }

            return response;

        }
        
        public async Task<Response> SelectUserProfileRole(int userID)
        {
            string sqlStatement = "SELECT role FROM dbo.UserProfiles WHERE userID = @userID";
            /*
            var connection = new SqlConnection(_connectionString);
            cmd.Parameters.AddWithValue("@ID", userID);
            */
            var response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlStatement,connection);
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string role = reader.GetString(reader.GetOrdinal("role"));
                            response.data = role;
                            response.isSuccessful = true;
                        }
                    }
                    else
                    {
                        response.errorMessage = "No role associated with user ID";

                    }
                }
            }


            return response;
        }
        

        public async Task<Response> SelectUserHash(int userID)
        {
            var sqlstatement = "SELECT userHash FROM dbo.Users WHERE userID = @userID";
            var response = new Response();
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
                            response.data = userHash;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting User Hash";

                    }
                }
            }

            return response;
        } 
        
        // Events Start
        public Task<Response> InsertEvent(EventDTO eventDto)
        {
            var currentDate = DateTime.Now.Date;
            var connection = new SqlConnection(_connectionString);
            var insertSql = "INSERT INTO dbo.Events (title, description,eventCreated, userID, lat, lng) values(@title,@description,@eventCreated,@userID,@lat,@lng)";
            var cmd = new SqlCommand(insertSql, connection);
            cmd.Parameters.AddWithValue("@title", eventDto._title);
            cmd.Parameters.AddWithValue("@description", eventDto._description);
            cmd.Parameters.AddWithValue("@eventCreated",currentDate);
            cmd.Parameters.AddWithValue("@userID", eventDto._userID);
            cmd.Parameters.AddWithValue("@lat", eventDto._lat);
            cmd.Parameters.AddWithValue("@lng", eventDto._lng);
            return ExecuteSqlCommand(connection,cmd);
        }

        public async Task<List<EventDTO>> SelectUserCreatedEvents(int userID)
        {
            var sqlstatement = "SELECT title, description FROM Events WHERE userID = @userID AND disabled != 1";
            List<EventDTO> events = new List<EventDTO>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string title = reader.GetString(reader.GetOrdinal("title"));
                        string description = reader.GetString(reader.GetOrdinal("description"));
                        events.Add(new EventDTO(title, description));
                    }

                }

            }

            return events;

        }
        public async Task<List<EventDTO>> SelectAllEvents()
        {
            var sqlstatement = "SELECT title, description, eventID , lat, lng FROM Events WHERE disabled != 1";
            List<EventDTO> events = new List<EventDTO>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string title = reader.GetString(reader.GetOrdinal("title"));
                        string description = reader.GetString(reader.GetOrdinal("description"));
                        int eventID = reader.GetInt32(reader.GetOrdinal("eventID"));
                        double lat= reader.GetDouble(reader.GetOrdinal("lat"));
                        double lng = reader.GetDouble(reader.GetOrdinal("lng"));
                        events.Add(new EventDTO(title, description, lat, lng, eventID));
                    }

                }
            }
            
            return events;
        }

        public async Task<Response> SelectEventPin(int eventID)
        {
            var sqlstatement = "SELECT eventID FROM dbo.Events WHERE eventID = @eventID";
            var response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                // Open the connection async
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                
                cmd.Parameters.AddWithValue("@eventID", eventID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            int eVent = reader.GetInt32(reader.GetOrdinal("eventID"));
                            
                            response.data = eVent;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event ID";

                    }
                }
            }
            
            return response; 
        }


        public async Task<List<EventDTO>> SelectUserEvents(int userID)
        {
            var sqlstatement = "SELECT title, description FROM Events JOIN EventsJoined EJ on Events.eventID = EJ.eventID WHERE EJ.userID = @userID";
            List<EventDTO> events = new List<EventDTO>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string title = reader.GetString(reader.GetOrdinal("title"));
                        string description = reader.GetString(reader.GetOrdinal("description"));
                        events.Add(new EventDTO(title,description));
                    }
                    
                }

            }

            return events;
        }

        public async Task<List<EventDTO>> SelectJoinedEvents(int userID)
        {
            var sqlstatement = "SELECT eventID FROM dbo.EventsJoined WHERE userID = @userID";
            List<EventDTO> events = new List<EventDTO>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int eventID = reader.GetInt32(reader.GetOrdinal("eventID"));
                        events.Add(new EventDTO(eventID));
                    }
                    
                }

            }

            return events; 
        }

        public async Task<Response> SelectEventID(int userID)
        {
            var sqlstatement = "SELECT eventID FROM dbo.Events WHERE userID = @userID";
            var response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                // Open the connection async
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                
                cmd.Parameters.AddWithValue("@userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            int eventID = reader.GetInt32(reader.GetOrdinal("eventID"));
                            
                            response.data = eventID;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event ID";

                    }
                }
            }
            
            return response; 
        }

        public async Task<Response> SelectEventCount(int eventID)
        {
            var sqlstatement = "SELECT count FROM dbo.Events WHERE eventID = @eventID";
            var response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                // Open the connection async
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                
                // Sql to return the userHash associated with the passed in userID 
                cmd.Parameters.AddWithValue("@eventID", eventID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            int count = reader.GetInt32(reader.GetOrdinal("count"));
                            
                            response.data = count;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event count";

                    }
                }
            }

            return response; 
        }

        public Task<Response> IncrementEvent(int eventID)
        {
            // Sql statement to increment the value
            var sqlstatement = "UPDATE dbo.Events SET count = count + 1 WHERE eventID = @eventID";
            var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@eventID", eventID);
            return ExecuteSqlCommand(connection, cmd);
        }
        
        public Task<Response> InsertJoinEvent(int eventID, int userID)
        {
            var connection = new SqlConnection(_connectionString);
            var insertSql = "INSERT INTO dbo.EventsJoined (userID, eventID) values(@userID,@eventID)";
            var cmd = new SqlCommand(insertSql, connection);
            cmd.Parameters.AddWithValue("@eventID", eventID);
            cmd.Parameters.AddWithValue("@userID", userID);
            return ExecuteSqlCommand(connection,cmd);
        }

        public Task<Response> DecrementEvent(int eventID)
        {
            // Sql statement to increment the value
            var sqlstatement = "UPDATE dbo.Events SET count = count - 1 WHERE eventID = @eventID";
            var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@eventID", eventID);
            return ExecuteSqlCommand(connection, cmd);
        }
        
        public Task<Response> UpdateEventCount(int eventID, int count)
        {
            // Sql statement to increment the value
            var sqlstatement = "UPDATE dbo.Events SET count = @count WHERE eventID = @eventID";
            var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@eventID", eventID);
            cmd.Parameters.AddWithValue("@count", count);
            return ExecuteSqlCommand(connection, cmd);
        }


        public async Task<Response> SelectEventOwner(int eventID)
        {
            var sqlstatement = "SELECT userID FROM dbo.Events WHERE eventID = @eventID";
            var response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                // Open the connection async
                await connection.OpenAsync();
                var cmd = new SqlCommand(sqlstatement, connection);
                // Sql to return the userHash associated with the passed in userID 
                cmd.Parameters.AddWithValue("@eventID", eventID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            int owner = reader.GetInt32(reader.GetOrdinal("userID"));
                            
                            // Response obj stores the userHash value inside of the data property
                            response.data = owner;
                            response.isSuccessful = true;

                        }
                    }
                    else
                    {
                        response.errorMessage = "Error getting event count";

                    }
                }
            }

            return response;  
        }

        public Task<Response> UpdateEventTitle(string title, int eventID)
        {
            var sqlstatement = "UPDATE dbo.Events SET title = @title WHERE eventID = @eventID";
            var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@eventID", eventID);
            cmd.Parameters.AddWithValue("@title", title);
            return ExecuteSqlCommand(connection, cmd); 
        }

        public Task<Response> UpdateEventDescription(string description, int eventID)
        {
            var sqlstatement = "UPDATE dbo.Events SET description = @description WHERE eventID = @eventID";
            var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sqlstatement, connection);
            cmd.Parameters.AddWithValue("@eventID", eventID);
            cmd.Parameters.AddWithValue("@description", description);
            return ExecuteSqlCommand(connection, cmd); 
        }
        
        
        public async Task<Response> SelectEventDate(int userID)
        {
            var sqlstatement = "SELECT eventCreated FROM dbo.Events WHERE userID = @userID";
            var response = new Response();
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
                            // Date stored in type obj
                            response.data = reader.GetDateTime(reader.GetOrdinal("eventCreated"));
                            response.isSuccessful = true;
                        }
                    }
                    else
                    {
                        response.errorMessage = "Error Getting Event Date Created";

                    }
                }
            }
            
            return response; 
        }
        
        public Task<Response> UpdateEventToDisabled(int eventID)
         {
             var sqlstatement = "UPDATE dbo.Events SET disabled = 1 WHERE eventID = @eventID";
             var connection = new SqlConnection(_connectionString);
             var cmd = new SqlCommand(sqlstatement, connection);
             cmd.Parameters.AddWithValue("@eventID", eventID);
             
             return ExecuteSqlCommand(connection, cmd); 
         }

    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
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

        public Task<Response> InsertUser(UserAccount user)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var insertSql = "INSERT INTO dbo.Users (username, \"password\", \"disabled\", salt, userHash) values('" + user._username + "', '" + user._password + "', 0, '" + user._salt +  "', '" + user._userHash + "')";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new SqlCommand(insertSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
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
        public Task<Response> InsertUserProfile(UserProfile user)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var insertSql = "INSERT into dbo.UserProfiles(userID, firstname, lastname, \"address\", birthday, \"role\") values('" +
                    user._userID + "', '" + user._firstName + "', '" + user._lastName + "', '" +
                    user._address + "', '" + user._birthday + "', '" + user.Identity.AuthenticationType + "')";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new SqlCommand(insertSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
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
        public Task<Response> InsertUserHash(String userHash,int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var insertSql = "INSERT into dbo.UserHash(userHash, \"userID\") values('" +
                    userHash + "', '" + userID + "')";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new SqlCommand(insertSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
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
        public Task<Response> IncrementUserAccountDisabled(UserAccount userAccount)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response response = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var updateSql = "UPDATE dbo.Users set \"disabled\" += 1 Where userID = " + userAccount._userID + "";
                try
                {
                    var command = new SqlCommand(updateSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
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
        public  Task<Response> UpdateUserProfile(UserProfile user)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var updateSql = "UPDATE dbo.UserProfiles set firstname = '" + user._firstName + "', lastname = '" +
                    user._lastName + "', \"address\" = '" + user._address +
                    "', birthday = '" + user._birthday + "' Where userID = '" + user._userID + "'";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new SqlCommand(updateSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
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

        public Task<Response> ChangePassword(String username, String newPassword)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var updateSql = "UPDATE dbo.Users set password = '" + newPassword + "' where username = '" + username + "'";
                try
                {
                    var command = new SqlCommand(updateSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
                    }
                    else if(rows > 1)
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "Error, Multiple Accounts Affected";
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "No account found";
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

        public Task<Response> DeleteUserProfile(int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var deleteSql = "DELETE FROM dbo.UserProfiles WHERE userID = '" + userID + "';";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
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
                deleteSql = "DELETE FROM dbo.Users WHERE userID = '" + userID + "';";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
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
        public Task<Response> DeleteUser(string username)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var deleteSql = "DELETE FROM dbo.UserProfiles WHERE username = '" + username + "';";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
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
                deleteSql = "DELETE FROM dbo.Users WHERE username = '" + username + "';";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
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
                    " Where dbo.Users.username = '" + user._username + "' AND " + "password = '" + user._password + "'";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
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
                var selectSql = "Select LogLevel from dbo.Logs Where \"user\" = '" + username + "' AND " +
                    "\"timestamp\" >= DATEADD(day, -1, getDate()) order by \"timestamp\" asc";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
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

        public Task<Response> Count(String tableName, String countedCollumn, String[] collumnNames, String[] values)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var countSql = "SELECT COUNT(" + countedCollumn + ") FROM " + tableName + " WHERE ";
                if (collumnNames.Length == values.Length)
                {
                    for (int i = 0; i < collumnNames.Length; i++)
                    {
                        if (i != collumnNames.Length - 1)
                        {
                            countSql = countSql + collumnNames[i] + @" = '" + values[i] + @"' and ";
                        }
                        else
                        {
                            countSql = countSql + collumnNames[i] + @" = '" + values[i] + @"';";
                        }
                    }
                }
                else
                {
                    result.errorMessage = "There must be an equal ammount of collumnNames and values";
                }
                try
                {
                    var command = new SqlCommand(countSql, connection);
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

        public Task<Response> CountAll(String tableName, String countedCollumn)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var countSql = "SELECT COUNT(" + countedCollumn + ") FROM " + tableName;
                try
                {
                    var command = new SqlCommand(countSql, connection);
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

        public Task<Response> CountSalt(String salt)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Creates an Insert SQL statements using the collumn names and values given
                var countSql = "SELECT COUNT (salt) FROM dbo.Users Where salt = '" + salt + "'";
                try
                {
                    var command = new SqlCommand(countSql, connection);
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

        public Task<Response> SelectUserProfileTable(ref List<UserProfile> userProfiles, String roleName)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
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
            tcs.SetResult(result);
            return tcs.Task;
        }
        public Task<Response> SelectUserProfile(ref UserProfile userProfile, int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.UserProfiles WHERE userID = '" + userID + "'";
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
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<bool> IsValidUsername(String username)
        {
            var tcs = new TaskCompletionSource<bool>();
            String sqlStatement = "Select COUNT(Username) FROM dbo.Users WHERE Username = \'" + username + "\'";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    var command = new SqlCommand(sqlStatement, connect);
                    if ((int)command.ExecuteScalar() == 1)
                    {
                        tcs.SetResult(true);
                    }
                    else
                    {
                        tcs.SetResult(false);
                    }
                }
                catch (SqlException s) { }
            }
            return tcs.Task;
        }

        public Task<Response> CreateRecoveryRequest(int userID, String newPassword)
        {
            var response = new Response();
            var tcs = new TaskCompletionSource<Response>();
            String insertSql = "Insert into dbo.RecoveryRequests(userID, newPassword) " +
                "values (\'" + userID + "\', \'" + newPassword + "\')";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                var command = new SqlCommand(insertSql, connect);
                try
                {
                    if(command.ExecuteNonQuery() == 1)
                    {
                        response.isSuccessful = true;
                        response.errorMessage = "Account recovery request sent";
                    }
                }
                catch (SqlException s)
                {
                    if(s.Message.Contains("conflicted with the FOREIGN KEY constraint \"RR_ForeignKey_01\""))
                    {
                        response.isSuccessful = false;
                        response.errorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
                    }
                    else
                    {
                        response.isSuccessful = false;
                        response.errorMessage = s.Message;
                    }
                }
                catch (Exception e)
                {
                    response.isSuccessful = false;
                    response.errorMessage = e.Message;
                }
            }
            tcs.SetResult(response);
            return tcs.Task;
        }
        public Task<Response> SelectUserAccount(ref UserAccount userAccount, String username)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.Users WHERE username = '" + username +"'";
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
            tcs.SetResult(result);
            return tcs.Task;
        }
        public Task<Response> SelectUserAccountTable(ref List<UserAccount> userAccounts, String role)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
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

        public Task<Response> GetRecoveryRequests(ref List<int> requests)
        {
            var response = new Response();
            var tcs = new TaskCompletionSource<Response>();
            string sqlStatement = "SELECT * FROM dbo.RecoveryRequests WHERE fulfilled = 0 ORDER BY [TimeStamp]";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                try
                {
                    connect.Open();
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = 0;

                            int ordinal = reader.GetOrdinal("userID");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userId = reader.GetInt32(ordinal);
                            }
                            requests.Add(userId);
                        }
                        reader.Close();
                        response.isSuccessful = true;
                    }
                    connect.Close();
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

        public Task<Response> GetNewPassword(int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string sqlSelect = "Select Top 1 newpassword, [timestamp] from dbo.RecoveryRequests WHERE fulfilled = 0 AND userId = " + userID + " Order by [timestamp] desc; ";
                try
                {
                    var command = new SqlCommand(sqlSelect, connect);
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
                var selectSql = "Select LogLevel from dbo.Logs Where \"user\" = '" + userId + "' AND " +
                    "\"timestamp\" >= DATEADD(day, -1, getDate()) order by \"timestamp\" asc";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
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

        public Task<Response> ResetAccount(int userID, String newPassword)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var updateSql = "UPDATE dbo.Users set password = '" + newPassword + "', \"disabled\" = 0 where userID = " + userID;
                try
                {
                    var command = new SqlCommand(updateSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
                    }
                    else if (rows > 1)
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "Error, Multiple Accounts Affected";
                        tcs.SetResult(result);
                        return tcs.Task;
                    }
                    else if (rows == 0)
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
                        tcs.SetResult(result);
                        return tcs.Task;
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

        public Task<Response> RequestFulfilled(int userID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string fulfill = "UPDATE dbo.RecoveryRequests SET fulfilled = 1 WHERE userID = " + userID;
                try
                {
                    var command = new SqlCommand(fulfill, connect);
                    if (command.ExecuteNonQuery() > 1)
                    {
                        result.isSuccessful = true;
                        result.errorMessage = "Account recovery completed successfully for user";
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "No Request for User Found";
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

        public Task<List<Pin>> SelectPinTable()
        {
            var tcs = new TaskCompletionSource<List<Pin>>();
            List<Pin> pins = new List<Pin>();
            Response result = new Response();
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
            tcs.SetResult(pins);
            return tcs.Task;
        }

        public Task<Response> InsertPin(Pin pin)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var insertSql = "INSERT INTO dbo.Pins (userID,lat,lng,pinType,\"description\",\"disabled\",completed,\"dateTime\") values(" + pin._userID + ", '" + pin._lat + "', '" + pin._lng + "', " + pin._pinType + ", '" + pin._description + "', " + pin._disabled + ", " + pin._completed + ", '" + pin._dateTime + "')";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new SqlCommand(insertSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.errorMessage = "Stored New Pin Successfully.";
                        result.isSuccessful = true;
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

        public Task<Response> UpdatePinToComplete(int pinID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string fulfill = "UPDATE dbo.Pins SET completed = 1 WHERE pinID = " + pinID;
                try
                {
                    var command = new SqlCommand(fulfill, connect);
                    if (command.ExecuteNonQuery() > 1)
                    {
                        result.isSuccessful = true;
                        result.errorMessage = "Update Pin To Complete successfully for user";
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "No Request for Pin Found";
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

        public Task<Response> UpdatePinType(int pinID, int pinType)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string fulfill = "UPDATE dbo.Pins SET pinType = " + pinType + " WHERE pinID = " + pinID;
                try
                {
                    var command = new SqlCommand(fulfill, connect);
                    if (command.ExecuteNonQuery() > 1)
                    {
                        result.isSuccessful = true;
                        result.errorMessage = "Update Pin Type successfully for user";
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "No Request for Pin Found";
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

        public Task<Response> UpdatePinContent(int pinID, string description)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string fulfill = "UPDATE dbo.Pins SET \"description\" = '" + description + "' WHERE pinID = " + pinID;
                try
                {
                    var command = new SqlCommand(fulfill, connect);
                    if (command.ExecuteNonQuery() > 1)
                    {
                        result.isSuccessful = true;
                        result.errorMessage = "Update Pin Content successfully for user";
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "No Request for Pin Found";
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

        public Task<Response> UpdatePinToDisabled(int pinID)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string fulfill = "UPDATE dbo.Pins SET \"disabled\" = 1 WHERE pinID = " + pinID;
                try
                {
                    var command = new SqlCommand(fulfill, connect);
                    if (command.ExecuteNonQuery() > 1)
                    {
                        result.isSuccessful = true;
                        result.errorMessage = "Update Pin To Disabled successfully for user";
                    }
                    else
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "No Request for Pin Found";
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

        public Task<Response> GetNewLogins(ref int[] rows)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
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
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<Response> GetNewRegistrations(ref int[] rows)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
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

        public Task<Response> GetPinsAdded(ref int[] rows)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
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

        public Task<Response> GetPinPulls(ref int[] rows)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
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
    }
}
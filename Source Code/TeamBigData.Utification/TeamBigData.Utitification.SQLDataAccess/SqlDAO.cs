﻿using Microsoft.Data.SqlClient;
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
    public class SqlDAO : IDBInserter, IDBCounter, IDAO, IDBSelecter, IDBUpdater
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
                var updateSql = "UPDATE dbo.UserProfiles set \"disabled\" += 1 Where userID = '" + userAccount._userID + "'";
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

        public Task<Response> CreateRecoveryRequest(String username, String newPassword)
        {
            var response = new Response();
            var tcs = new TaskCompletionSource<Response>();
            String insertSql = "Insert into dbo.RecoveryRequests(username, newPassword) " +
                "values (\'" + username + "\', \'" + newPassword + "\')";
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
                            password = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("userHash");
                        if (!reader.IsDBNull(ordinal))
                        {
                            password = reader.GetString(ordinal);
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
                                password = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("userHash");
                            if (!reader.IsDBNull(ordinal))
                            {
                                password = reader.GetString(ordinal);
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

        public Task<Response> GetRecoveryRequests(ref List<String> requests)
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
                            String userName = "";

                            int ordinal = reader.GetOrdinal("username");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userName = reader.GetString(ordinal);
                            }
                            requests.Add(userName);
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

        public Task<Response> GetNewPassword(string username)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string sqlSelect = "Select Top 1 newpassword, [timestamp] from dbo.RecoveryRequests WHERE fulfilled = 0 AND username = \'" + username + "\' Order by [timestamp] desc; ";
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

        public Task<Response> CountUserLoginAttempts(String username)
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
            }
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<Response> ResetAccount(String username, String newPassword)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var updateSql = "UPDATE dbo.Users set password = '" + newPassword + "', \"disabled\" = 0 where username = \'" + username + "\'";
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
        /*
         * Functions not used
         * public Task<Response> GetUser(UserAccount user)
        {
            var tcs = new TaskCompletionSource<Response>();
            var list = new Object[8];
            Response result = new Response();
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var selectSql = "Select dbo.users.userID, \"disabled\", firstname, lastname, dbo.userprofiles.email, \"address\", " +
                    "birthday, role from dbo.Users left join dbo.UserProfiles on (dbo.Users.username = dbo.UserProfiles.username)" +
                    " Where dbo.Users.username = '" + user._username + "' AND " + "password = '" + user._password + "'";
                try
                {
                    var command = new SqlCommand(selectSql, connection);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        result.isSuccessful = true;
                        reader.GetValues(list);
                        if ((int)list[1] == 0)
                        {
                            var userProfile = new UserProfile((int)list[0], (string)list[2], (string)list[3], 21, (string)list[4],
                                (string)list[5], ((DateTime)list[6]), new GenericIdentity((string)list[0], (string)list[7]));
                            result.data = userProfile;
                        }
                        else
                        {
                            result.isSuccessful = false;
                            result.errorMessage = "Error: Account disabled. Perform account recovery or contact system admin";
                        }
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
        */

        public Task<Response> RequestFulfilled(string username)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                string fulfill = "UPDATE dbo.RecoveryRequests SET fulfilled = 1 WHERE username = \'" + username + "\'; ";
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
    }
}
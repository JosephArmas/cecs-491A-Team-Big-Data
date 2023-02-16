using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using static System.Net.WebRequestMethods;


namespace TeamBigData.Utification.SQLDataAccess
{
    public class SqlDAO : IDBInserter, IDBCounter, IDAO, IDBSelecter, IDBDeleter, IDBUpdater
    {
        private String _connectionString;

        public SqlDAO(String connectionString)
        {
            _connectionString = connectionString;
        }

        public Response InsertUser(UserAccount user)
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
                return tcs.Task.Result;
            }
        }

        public Response InsertUserProfile(UserProfile user)
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
                return tcs.Task.Result;
            }
        }
        public Response InsertUserHash(String userHash,int userID)
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
                return tcs.Task.Result;
            }
        }
        public async Task<Response> UpdateUserProfile(UserProfile user)
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
                return tcs.Task.Result;
            }
        }

        public async Task<Response> DeleteUserProfile(int userID)
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
                return tcs.Task.Result;
            }
        }

        public async Task<Response> Count(String tableName, String countedCollumn, String[] collumnNames, String[] values)
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
                return tcs.Task.Result;
            }
        }
        public async Task<Response> CountAll(String tableName, String countedCollumn)
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
                return tcs.Task.Result;
            }
        }

        public async Task<Response> CountSalt(String salt)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
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
                return tcs.Task.Result;
            }
        }

        public async Task<Response> Execute(object req)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
            if (req.GetType() != typeof(string)) //Verifies if the parameter matches the acceptable string format type
            {
                result.errorMessage = "Error: input parameter for SqlDAO not of type string";
                result.isSuccessful = false;
                tcs.SetResult(result);
                return tcs.Task.Result;
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
                return tcs.Task.Result;
            }
        }

        public Response SelectUserProfileTable(ref List<UserProfile> userProfiles)
        {
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.UserProfile";
            using (SqlConnection connect = new SqlConnection(_connectionString))
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
                        ordinal = reader.GetOrdinal("First Name");
                        if (!reader.IsDBNull(ordinal))
                        {
                           firstName  = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("Last Name");
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
                        userProfiles.Add(new UserProfile(userID, firstName, lastName, age, address, birthday, new GenericIdentity(userID.ToString(), role)));
                    }
                    reader.Close();
                }
                connect.Close();
                if(userProfiles.Count > 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "Returning List of UserProfiles";
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Empty List of UserProfiles";
                }
            }
            return result;
        }

        public Response SelectUserProfile(int userID, ref UserProfile userProfile)
        {
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.UserProfiles WHERE userID = '" + userID + "'";
            using (SqlConnection connect = new SqlConnection(_connectionString))
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
                        userProfile = new UserProfile(userID, firstName, lastName, age, address, birthday, new GenericIdentity(userID.ToString(), role));
                    }
                    reader.Close();
                }
                connect.Close();
            }
            if (userProfile._userID > 0)
            {
                result.isSuccessful = true;
                result.errorMessage = "UserProfile Found";
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "No UserProfile Found";
            }
            return result;
        }

        public Response SelectUserAccount(String username, ref UserAccount userAccount)
        {
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.Users WHERE username = '" + username +"'";
            using (SqlConnection connect = new SqlConnection(_connectionString))
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
                        userAccount = new UserAccount(userID, userName, password, salt, userHash, verified);
                    }
                    reader.Close();
                }
                connect.Close();
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
            return result;
        }

        public Response SelectUserAccountTable(ref List<UserAccount> userAccounts, string role)
        {
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.Users";
            using (SqlConnection connect = new SqlConnection(_connectionString))
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
            return result;
        }
        public Response SelectLastUserID()
        {
            var tcs = new TaskCompletionSource<Response>();
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
                        tcs.Task.Result.data = rows.GetInt32(0);
                        tcs.Task.Result.isSuccessful = true;
                    }
                }
                catch (SqlException s)
                {
                    tcs.Task.Result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    tcs.Task.Result.errorMessage = e.Message;
                }
            }

            return tcs.Task.Result;
        }

        public async Task<Response> CountUserLoginAttempts(String username)
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
                return tcs.Task.Result;
            }
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

        
         * 
         */
    }
}
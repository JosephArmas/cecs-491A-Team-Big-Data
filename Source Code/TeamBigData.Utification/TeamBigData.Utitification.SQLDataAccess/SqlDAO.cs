using Azure.Identity;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;


namespace TeamBigData.Utification.SQLDataAccess
{
    public class SqlDAO : IDBInserter, IDBCounter, IDAO, IDBSelecter
    {
        private String _connectionString;

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
                var insertSql = "INSERT INTO dbo.Users (username, \"password\") values('" + user._username + "', '" + user._password + "')";
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
                var insertSql = "INSERT into dbo.UserProfiles(username, firstname, lastname, email, \"address\", birthday, role) values('" +
                    user._username + "', '" + user._firstName + "', '" + user._lastName + "', '" + user._email + "', '" +
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

        public Task<Response> UpdateUserProfile(UserProfile user)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var updateSql = "UPDATE dbo.UserProfiles set firstname = '" + user._firstName + "', lastname = '" +
                    user._lastName + "', email = '" + user._email + "', \"address\" = '" + user._address +
                    "', birthday = '" + user._birthday + "' Where username = '" + user._username + "'";
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

        public Task<Response> DeleteUser(UserProfile user)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var deleteSql = "DELETE FROM dbo.UserProfiles WHERE username = '" + user._username + "';";
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
                deleteSql = "DELETE FROM dbo.Users WHERE username = '" + user._username + "';";
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
                var selectSql = "Select dbo.users.username, \"disabled\", firstname, lastname, dbo.userprofiles.email, \"address\", " +
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
                            var userProfile = new UserProfile((string)list[0], (string)list[2], (string)list[3], 21, (string)list[4], 
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

        public Task<Response> SelectUserProfileTable(ref List<UserProfile> list)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.UserProfiles";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                try
                {
                    using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                    {
                        // read through all rows
                        while (reader.Read())
                        {
                            String userName = "";
                            String firstName = "";
                            String lastName = "";
                            int age = 0;
                            String email = "";
                            String address = "";
                            DateTime birthday = new DateTime();
                            String role = "";

                            int ordinal = reader.GetOrdinal("Username");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userName = reader.GetString(ordinal);
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
                            var userProfile = new UserProfile(userName, firstName, lastName, age, email, address, birthday, new GenericIdentity("", role));
                            list.Add(userProfile);
                        }
                        result.isSuccessful = true;
                        reader.Close();
                    }
                    connect.Close();
                }
                catch(SqlException s)
                {
                    result.isSuccessful = false;
                    result.errorMessage = s.Message;
                }
                catch(Exception e)
                {
                    result.isSuccessful = false;
                    result.errorMessage = e.Message;
                }
            }
            tcs.SetResult(result);
            return tcs.Task;
        }

        public Task<UserProfile> SelectUserProfile(String username)
        {
            var tcs = new TaskCompletionSource<UserProfile>();
            UserProfile userProfile = new UserProfile("");
            string sqlStatement = "SELECT * FROM dbo.UserProfiles WHERE Username = \'" + username + "\'";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                {
                    // read through all rows
                    while (reader.Read())
                    {
                        String userName = "";
                        String firstName = "";
                        String lastName = "";
                        int age = 0;
                        String email = "";
                        String address = "";
                        DateTime birthday = new DateTime();
                        String role = "";

                        int ordinal = reader.GetOrdinal("Username");
                        if (!reader.IsDBNull(ordinal))
                        {
                            userName = reader.GetString(ordinal);
                        }

                        ordinal = reader.GetOrdinal("Role");
                        if (!reader.IsDBNull(ordinal))
                        {
                            role = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("First Name");
                        if (!reader.IsDBNull(ordinal))
                        {
                            firstName = reader.GetString(ordinal);
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
                        userProfile = new UserProfile(userName, firstName, lastName, age, email, address, birthday, new GenericIdentity("", role));
                        tcs.SetResult(userProfile);
                    }
                    reader.Close();
                }
                connect.Close();
            }
            return tcs.Task;
        }

        public Task<UserAccount> SelectUserAccount(String username)
        {
            var tcs = new TaskCompletionSource<UserAccount>();
            UserAccount userAccount = new UserAccount("","");
            string sqlStatement = "SELECT * FROM dbo.UserAccount WHERE Username = \'" + username + "\'";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                {
                    // read through all rows
                    while (reader.Read())
                    {
                        String userName = "";
                        String password = "";
                        String otp = "";
                        String otpCreated = DateTime.Now.Ticks.ToString();
                        bool verified = false;

                        int ordinal = reader.GetOrdinal("Username");
                        if (!reader.IsDBNull(ordinal))
                        {
                            userName = reader.GetString(ordinal);
                        }

                        ordinal = reader.GetOrdinal("Password");
                        if (!reader.IsDBNull(ordinal))
                        {
                            password = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("OTP");
                        if (!reader.IsDBNull(ordinal))
                        {
                            otp = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("OTP Created");
                        if (!reader.IsDBNull(ordinal))
                        {
                            otpCreated = reader.GetString(ordinal);
                        }
                        ordinal = reader.GetOrdinal("Verified");
                        if (!reader.IsDBNull(ordinal))
                        {
                            verified = reader.GetBoolean(ordinal);
                        }
                        userAccount = new UserAccount(userName, password, otp, otpCreated, verified);
                        tcs.SetResult(userAccount);
                    }
                    reader.Close();
                }
                connect.Close();
            }
            return tcs.Task;
        }

        public Task<bool> IsValidUsername(String username)
        {
            var tcs = new TaskCompletionSource<bool>();
            String sqlStatement = "Select COUNT(Username) FROM dbo.Users WHERE Username = \'" + username + "\'";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                var command = new SqlCommand(sqlStatement, connect);
                if((int)command.ExecuteScalar() == 1)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetResult(false);
                }
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

        public Task<Response> SelectUserAccountTable(ref List<UserAccount> accountList)
        {
            var tcs = new TaskCompletionSource<Response>();
            UserAccount userAccount = new UserAccount("", "");
            Response result = new Response();
            string sqlStatement = "SELECT * FROM dbo.Users";
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                {
                    try
                    {
                        // read through all rows
                        while (reader.Read())
                        {
                            String userName = "";
                            String password = "";
                            int disabled = 0;
                            int ordinal = reader.GetOrdinal("Username");
                            if (!reader.IsDBNull(ordinal))
                            {
                                userName = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("Password");
                            if (!reader.IsDBNull(ordinal))
                            {
                                password = reader.GetString(ordinal);
                            }
                            ordinal = reader.GetOrdinal("Disabled");
                            if (!reader.IsDBNull(ordinal))
                            {
                                disabled = reader.GetInt32(ordinal);
                            }
                            userAccount = new UserAccount(userName, password);
                            accountList.Add(userAccount);
                        }
                        reader.Close();
                        result.isSuccessful = true;
                    }
                    catch(SqlException s)
                    {
                        result.isSuccessful = false;
                        result.errorMessage = s.Message;
                    }
                    catch(Exception e)
                    {
                        result.isSuccessful = false;
                        result.errorMessage = e.Message;
                    }
                }
                connect.Close();
            }
            tcs.SetResult(result);
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
                    response.isSuccessful = false;
                    response.errorMessage = s.Message;
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
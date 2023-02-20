// using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Data.SqlClient;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using MySql.Data.MySqlClient;


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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                // var insertSql = "INSERT INTO dbo.Users (username, \"password\") values('" + user._username + "', '" + user._password + "')";
                var insertSql = "INSERT INTO dbo.Users (username, password) values('" + user._username + "', '" + user._password + "')";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new MySqlCommand(insertSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
                    }
                }
                catch (MySqlException s)
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                // var insertSql = "INSERT into dbo.UserProfiles(username, firstname, lastname, email, \"address\", birthday, role) values('" +
                //     user._username + "', '" + user._firstName + "', '" + user._lastName + "', '" + user._email + "', '" +
                //     user._address + "', '" + user._birthday + "', '" + user.Identity.AuthenticationType + "')";
                var insertSql = "INSERT into dbo.UserProfiles (username, firstname, lastname, email, address, birthday, role) values('" +
                    user._username + "','" + user._firstName + "','" + user._lastName + "','" + user._email + "','" +
                    user._address + "','" + user._cbirthday + "','" + user.Identity.AuthenticationType + "')";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new MySqlCommand(insertSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
                    }
                }
                catch (MySqlException s)
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var updateSql = "UPDATE dbo.UserProfiles set firstname = '" + user._firstName + "', lastname = '" +
                    user._lastName + "', email = '" + user._email + "', address = '" + user._address +
                    "', birthday = '" + user._cbirthday + "' Where username = '" + user._username + "'";
                //Executes the SQL Insert Statement using the Connection String provided
                try
                {
                    var command = new MySqlCommand(updateSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        result.isSuccessful = true;
                    }
                }
                catch (MySqlException s)
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var deleteSql = "DELETE FROM dbo.UserProfiles WHERE username = '" + user._username + "';";
                try
                {
                    var command = new MySqlCommand(deleteSql, connection);
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
                    var command = new MySqlCommand(deleteSql, connection);
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                // var selectSql = "Select dbo.users.username, \"disabled\", firstname, lastname, dbo.userprofiles.email, \"address\", " +
                //     "birthday, role from dbo.Users left join dbo.UserProfiles on (dbo.Users.username = dbo.UserProfiles.username)" +
                //     " Where dbo.Users.username = '" + user._username + "' AND " + "password = '" + user._password + "'";
                var selectSql = "Select dbo.Users.username, disabled, firstname, lastname, dbo.UserProfiles.email, address, " +
                "birthday, role from dbo.Users left join dbo.UserProfiles on (dbo.Users.username = dbo.UserProfiles.username)" +
                " Where dbo.Users.username = '" + user._username + "' AND " + "password = '" + user._password + "'";
                try
                {
                    var command = new MySqlCommand(selectSql, connection);
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
                catch (MySqlException s)
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given for mssql
                // var selectSql = "Select LogLevel from dbo.Logs Where \"user\" = '" + username + "' AND " +
                //     "\"timestamp\" >= DATEADD(day, -1, getDate()) order by \"timestamp\" asc";
                // create an Insert SQL for mariadb
                var selectSql = "Select LogLevel FROM dbo.Logs WHERE user = '" + username + "' AND " +
                    "`TimeStamp` >= DATE_SUB(NOW(), INTERVAL 1 DAY) ORDER BY `TimeStamp` ASC";
                try
                {
                    var command = new MySqlCommand(selectSql, connection);
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
            using (var connection = new MySqlConnection(_connectionString))
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
                    var command = new MySqlCommand(countSql, connection);
                    result.data = command.ExecuteScalar();
                    result.isSuccessful = true;
                }
                catch (MySqlException s)
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var countSql = "SELECT COUNT(" + countedCollumn + ") FROM " + tableName;
                try
                {
                    var command = new MySqlCommand(countSql, connection);
                    result.data = command.ExecuteScalar();
                    result.isSuccessful = true;
                }
                catch (MySqlException s)
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
            using (MySqlConnection connect = new MySqlConnection(_connectionString.ToString()))
            {
                connect.Open();
                try
                {
                    result.data = (new MySqlCommand(req.ToString(), connect)).ExecuteNonQuery();
                    result.isSuccessful = true;
                }
                catch (MySqlException s)
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

        public List<UserProfile> SelectUserProfileTable()
        {
            List<UserProfile> list = new List<UserProfile>();
            string sqlStatement = "SELECT * FROM dbo.UserProfile";
            using (MySqlConnection connect = new MySqlConnection(_connectionString))
            {
                connect.Open();
                using (var reader = (new MySqlCommand(sqlStatement, connect)).ExecuteReader())
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
                        var userProfile = new UserProfile(userName,firstName, lastName, age, email, address, birthday, new GenericIdentity("", role));
                        list.Add(userProfile);
                    }
                    reader.Close();
                }
                connect.Close();
            }
            return list;
        }

        public UserProfile SelectUserProfile(String username)
        {
            UserProfile userProfile = new UserProfile("");
            string sqlStatement = "SELECT * FROM dbo.UserProfiles WHERE Username = " + username;
            using (MySqlConnection connect = new MySqlConnection(_connectionString))
            {
                connect.Open();
                using (var reader = (new MySqlCommand(sqlStatement, connect)).ExecuteReader())
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
                    }
                    reader.Close();
                }
                connect.Close();
            }
            return userProfile;
        }

        public UserAccount SelectUserAccount(String username)
        {
            UserAccount userAccount = new UserAccount("","");
            string sqlStatement = "SELECT * FROM dbo.UserAccount WHERE Username = " + username;
            using (MySqlConnection connect = new MySqlConnection(_connectionString))
            {
                connect.Open();
                using (var reader = (new MySqlCommand(sqlStatement, connect)).ExecuteReader())
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
                    }
                    reader.Close();
                }
                connect.Close();
            }
            return userAccount;
        }

        public List<UserAccount> SelectUserAccountTable()
        {
            List<UserAccount> userAccounts = new List<UserAccount>();
            return userAccounts;
        }
    }
}
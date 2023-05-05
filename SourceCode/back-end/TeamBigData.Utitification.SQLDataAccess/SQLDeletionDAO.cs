using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SQLDeletionDAO //:IDBDeleter
    {
        private String _connectionString;
        /*
        public SQLDeletionDAO(String connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Deletes the features through one sql delete statement. Borrowed from the DeleteUser method from SqlDAO by David.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The response from the sql query</returns>
        public Task<Response> DeleteFeatureInfo(UserProfile user)
        {
            var tcs = new TaskCompletionSource<Response>();
            var username = user;
            Response result = new Response();
            result.IsSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the column names and values given
                var deleteSql = "DELETE FROM dbo.\"Events\" WHERE userID = '" + username._userID + "';DELETE FROM dbo.ProfilePic WHERE userID = '" + username._userID + "'" +
                    ";DELETE FROM dbo.\"Services\" WHERE userID = '" + username._userID + "';" +
                    "delete pics from dbo.pins pin inner join dbo.pinpic pics on (pin.pinID = pics.pinID) where pin.userID = \'" + username._userID + "\';" +
                    "delete from dbo.pins where userID = \'" + username._userID + "\';";
                //TODO: delete associated pinpics
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
                    result.IsSuccessful = true;
                    result.Data = rows;
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
                Console.WriteLine(result.ErrorMessage);
                tcs.SetResult(result);
                return tcs.Task;
            }
        }
        /// <summary>
        /// Deletes the user information from users table and userprofiles through one sql statement. Borrowed from the DeleteUser method from SqlDAO by David
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The response from the sql query</returns>
        public Task<Response> DeleteUser(UserProfile user)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            var username = user;
            result.IsSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the column names and values given
                var deleteSql = "DELETE FROM dbo.UserProfiles WHERE userID = '" + username._userID + "';DELETE FROM dbo.Users WHERE userID = '" + username._userID + "';";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        result.IsSuccessful = true;
                        result.Data = rows;
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
        }*/
    }
}

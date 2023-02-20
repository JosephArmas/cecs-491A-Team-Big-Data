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
    public class SQLDeletionDAO : IDBDeleter
    {
        private String _connectionString;

        public SQLDeletionDAO(String connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Deletes the features through one sql delete statement. Borrowed from the DeleteUser method from SqlDAO by David.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The response from the sql query</returns>
        public Task<Response> DeleteFeatureInfo(String user)
        {
            var tcs = new TaskCompletionSource<Response>();
            var username = user;
            Response result = new Response();
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the column names and values given
                Console.WriteLine("Username is: "+user);
                var deleteSql = "DELETE FROM dbo.Events WHERE username = '" + username + "';DELETE FROM dbo.Pictures WHERE username = '" + username + "'" +
                    ";DELETE FROM dbo.Services WHERE username = '" + username + "';DELETE FROM dbo.Pins WHERE username = '" + username + "';";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
                    result.isSuccessful = true;
                    result.data = rows;
                    Console.WriteLine("The feature rows affected->>" + rows);
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
        /// <summary>
        /// Deletes the user information from users table and userprofiles through one sql statement. Borrowed from the DeleteUser method from SqlDAO by David
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The response from the sql query</returns>
        public Task<Response> DeleteUser(String user)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            var username = user;
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the column names and values given
                var deleteSql = "DELETE FROM dbo.UserProfiles WHERE username = '" + username + "';DELETE FROM dbo.Users WHERE username = '" + username + "';";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        result.isSuccessful = true;
                        result.data = rows;
                    }
                    Console.WriteLine("The rows affected->>"+rows);
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

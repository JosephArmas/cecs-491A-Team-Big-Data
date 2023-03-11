using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SqlDAO : IDBInserter, IDBDeleter, IDBClear, IDAO
    {
        private String _connectionString;

        public SqlDAO(String connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<Response> Insert(String tableName, String[] values)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var insertSql = "INSERT INTO " + tableName + " values(";
                foreach (String s in values)
                {
                    insertSql += @"'" + s + @"', ";
                }
                insertSql = insertSql.Remove(insertSql.Length - 2);
                insertSql += ");";
                try
                {
                    var command = new SqlCommand(insertSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if(rows == 1)
                    {
                        result.isSuccessful = true;
                    }
                }
                catch(SqlException s)
                {
                    result.errorMessage = s.Message;
                }
                catch(Exception e)
                {
                    result.errorMessage = e.Message;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }

        public Task<Response> Insert(String tableName, String[] collumnNames, String[] values)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var insertSql = "INSERT INTO " + tableName + "(";
                foreach (String c in collumnNames)
                {
                    insertSql = insertSql + c + ", ";
                }
                insertSql = insertSql.Remove(insertSql.Length - 2);
                insertSql += ");" + " values(";
                foreach (String s in values)
                {
                    insertSql += @"'" + s + @"', ";
                }
                insertSql = insertSql.Remove(insertSql.Length - 2);
                insertSql += ");";
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

        public Task<Response> Clear(String tableName)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var deleteSql = "DELETE FROM " + tableName + ";";
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows > 0)
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

        public Task<Response> Delete(String tableName, String[] collumnNames, String[] values)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            result.isSuccessful = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Creates an Insert SQL statements using the collumn names and values given
                var deleteSql = "DELETE FROM " + tableName + "WHERE ";
                if (collumnNames.Length == values.Length)
                {
                    for (int i = 0; i < collumnNames.Length; i++)
                    {
                        if (i != collumnNames.Length - 1)
                        {
                            deleteSql = deleteSql + collumnNames[i] + @" = '" + values[i] + @"' and ";
                        }
                        deleteSql = deleteSql + collumnNames[i] + @" = '" + values[i] + @"';";
                    }
                }
                else
                {
                    result.errorMessage = "There must be an equal ammount of collumnNames and values";
                }
                try
                {
                    var command = new SqlCommand(deleteSql, connection);
                    var rows = command.ExecuteNonQuery();
                    if (rows > 0)
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
        public Task<Response> Execute(object req)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
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
                var resu = (new SqlCommand(req.ToString(), connect)).ExecuteNonQuery();
                if (resu == 1)
                {
                    result.isSuccessful = true;
                }
                else
                {
                    result.isSuccessful = false;
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }
    }
}

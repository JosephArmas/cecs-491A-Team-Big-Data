using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SqlDAO : IDBInserter, IDBDeleter, IDBClear
    {
        private String _connectionString;

        public SqlDAO(String connectionString)
        {
            _connectionString = connectionString;
        }

        public Response Insert(String tableName, String[] values)
        {
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
                return result;
            }
        }

        public Response Insert(String tableName, String[] collumnNames, String[] values)
        {
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
                return result;
            }
        }

        public Response Clear(String tableName)
        {
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
                return result;
            }
        }

        public Response Delete(String tableName, String[] collumnNames, String[] values)
        {
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
                return result;
            }
        }
    }
}

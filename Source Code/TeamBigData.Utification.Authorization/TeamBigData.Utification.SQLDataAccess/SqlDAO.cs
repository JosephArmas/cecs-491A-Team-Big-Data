using System.Data.SqlClient;
using System.Security;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstraction;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SqlDAO : IDBInserter, IDBDeleter, IDBClear, IDBCounter, IDAO, IDBSelecter
    {
        public string ConnectionString { get; set; }
        public SqlDAO(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public Task<Response> Clear(string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Count(string tableName, string countedCollumn, string[] collumnNames, string[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(string tableName, string[] collumnNames, string[] values)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Execute(object req)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Insert(string tableName, string[] values)
        {
            throw new NotImplementedException();
        }

        public Task<Response> SelectTable(List<object> list, object req)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response result = new Response();
            if (req.GetType()!=typeof(string))
            {
                result.errorMessage = "Error: input parameter for SqlDAO not of type string";
                result.isSuccessful = false;
                tcs.SetResult(result);
                return tcs.Task;
            }
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                connect.Open();
                using (var reader = (new SqlCommand(req.ToString(), connect)).ExecuteReader())
                {
                    // read through all rows
                    while (reader.Read())
                    {
                        var userProfile = new UserProfile();

                        int ordinal = reader.GetOrdinal("UserName");
                        if (!reader.IsDBNull(ordinal)) {
                            userProfile.UserName = reader.GetString(ordinal);
                        }

                        ordinal = reader.GetOrdinal("FullName");
                        if (!reader.IsDBNull(ordinal))
                        {
                            userProfile.Fullname = reader.GetString(ordinal);
                        }

                        ordinal = reader.GetOrdinal("Birthday");
                        if (!reader.IsDBNull(ordinal))
                        {
                            userProfile.Birthday =  reader.GetString(ordinal);
                        }

                        ordinal = reader.GetOrdinal("Role");
                        if (!reader.IsDBNull(ordinal))
                        {
                            userProfile.Role = reader.GetString(ordinal);
                        }

                        list.Add(userProfile);
                    }
                    // check if read through all rows
                    if (reader.Read())
                    {
                        result.isSuccessful = false;
                        result.errorMessage = "Failed to read all rows";
                    }
                    reader.Close();
                }
                tcs.SetResult(result);
                return tcs.Task;
            }
        }
    }
}
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

        public List<UserProfile> GetUserProfileTable()
        {
            List<UserProfile> list = new List<UserProfile>();
            string sqlStatement = "SELECT * FROM dbo.UserProfile";
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                connect.Open();
                using (var reader = (new SqlCommand(sqlStatement, connect)).ExecuteReader())
                {
                    // read through all rows
                    while (reader.Read())
                    {
                        var userProfile = new UserProfile("");

                        int ordinal = reader.GetOrdinal("Role");
                        if (!reader.IsDBNull(ordinal))
                        {
                            userProfile = new UserProfile(new GenericIdentity("",reader.GetString(ordinal)));
                        }

                        ordinal = reader.GetOrdinal("UserName");
                        if (!reader.IsDBNull(ordinal)) {
                            userProfile.Username = reader.GetString(ordinal);
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
                        list.Add(userProfile);
                    }
                    reader.Close();
                }
                connect.Close();
            }
            return list;
        }
    }
}
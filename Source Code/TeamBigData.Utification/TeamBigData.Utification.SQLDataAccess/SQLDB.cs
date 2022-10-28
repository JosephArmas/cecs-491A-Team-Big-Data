using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using TeamBigData.Utification.Registration;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SQLDB
    {
        public Response InsertUser(User user)
        {
            Response result = new Response();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";//Can also add Encrypt=true at end
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var insertSql = "INSERT INTO dbo.Users (username, password, email) values(@username, @password, @email)";

                var parameter = new SqlParameter("@username", user.GetUsername());
                var parameter2 = new SqlParameter("@password", user.GetPassword());
                var parameter3 = new SqlParameter("@email", user.GetEmail());
                var command = new SqlCommand(insertSql, connection);
                command.Parameters.Add(parameter);
                command.Parameters.Add(parameter2); 
                command.Parameters.Add(parameter3);
                

                //Execute SQL Command
                var rows = command.ExecuteNonQuery();
                if (rows == 1)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "No Error";
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Unknown Database Error";
                }

                return result;
            }
        }
    }
}
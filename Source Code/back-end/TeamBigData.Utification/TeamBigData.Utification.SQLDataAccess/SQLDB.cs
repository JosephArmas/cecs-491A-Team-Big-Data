using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace TeamBigData.Utification.SQLDataAccess
{
    public class SQLDB
    {
        public Response InsertUser(String username, String password, String email)
        {
            Response result = new Response();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";//Can also add Encrypt=true at end
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var insertSql = "INSERT INTO dbo.Users (username, password, email) values(@username, @password, @email)";

                var parameter = new SqlParameter("@username", username);
                var parameter2 = new SqlParameter("@password", password);
                var parameter3 = new SqlParameter("@email", email);
                var command = new SqlCommand(insertSql, connection);
                command.Parameters.Add(parameter);
                command.Parameters.Add(parameter2);
                command.Parameters.Add(parameter3);


                //Execute SQL Command
                try
                {
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
                }
                catch(SqlException e)
                {
                    result.isSuccessful = false;
                    if(e.Message.Contains("PRIMARY KEY"))
                    {
                        result.errorMessage = "Username taken, please pick a new username";
                    }
                    else if(e.Message.Contains("UNIQUE"))
                    {
                        result.errorMessage = "Email already linked to an account, please pick a new email";
                    }
                    else if(e.Message.Contains("Syntx"))
                    {
                        result.errorMessage = "Syntax Error in the SQL Statement";
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                        result.errorMessage = "Unknown SQL Exception";
                    }
                }
                return result;
            }
        }
        public Response InsertTestUser(String username, String password, String email)
        {
            Response result = new Response();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";//Can also add Encrypt=true at end
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var insertSql = "INSERT INTO dbo.TestUsers (username, password, email) values(@username, @password, @email)";

                var parameter = new SqlParameter("@username", username);
                var parameter2 = new SqlParameter("@password", password);
                var parameter3 = new SqlParameter("@email", email);
                var command = new SqlCommand(insertSql, connection);
                command.Parameters.Add(parameter);
                command.Parameters.Add(parameter2);
                command.Parameters.Add(parameter3);


                //Execute SQL Command
                try
                {
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
                }
                catch (SqlException e)
                {
                    result.isSuccessful = false;
                    if (e.Message.Contains("PRIMARY KEY"))
                    {
                        result.errorMessage = "Username taken, please pick a new username";
                    }
                    else if (e.Message.Contains("UNIQUE"))
                    {
                        result.errorMessage = "Email already linked to an account, please pick a new email";
                    }
                    else if (e.Message.Contains("Syntx"))
                    {
                        result.errorMessage = "Syntax Error in the SQL Statement";
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                        result.errorMessage = "Unknown SQL Exception";
                    }
                }
                return result;
            }
        }
        public Response ClearTestUsers()
        {
            Response result = new Response();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";//Can also add Encrypt=true at end
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var insertSql = "Delete From dbo.TestUsers";
                var command = new SqlCommand(insertSql, connection);
                var deleted = command.ExecuteNonQuery();
                if (deleted > 0)
                {
                    result.isSuccessful = true;
                    result.errorMessage = "No Error";
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Error Clearing Test Users Database";
                }
            }
            return result;
        }
    }
}
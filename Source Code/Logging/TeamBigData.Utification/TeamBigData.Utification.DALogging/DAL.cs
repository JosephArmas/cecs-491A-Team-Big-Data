using Microsoft.Data.SqlClient;
using TeamBigData.Utification.Cross;
namespace TeamBigData.Utification.DALogging
{
    public class DAL
    {
        static string connectionString = @"Server=.;Database=TeamBigData.Utification.Logs;Integrated Security=True;TrustServerCertificate=True;Encrypt=True";
        SqlConnection connection = new SqlConnection(connectionString);
        public Results realLog(string message)
        {
            //TODO: Log to Database
            //Step 1: Validation
            //Step 2: Logging
            connection.Open();
            var insertSql = "INSERT INTO dbo.Logs (Message) VALUES ('"+message+"')";
            var command = new SqlCommand(insertSql, connection);
            var rows = command.ExecuteNonQuery();
            connection.Close();
            if (rows == 1)
            {
                return Results.Pass;
            }
            return Results.Fail;
        }
        public Results Log(string datetime,Loglevel level,string opr,Category cat,string message)
        {
            //TODO: Log to Database
            //Step 1: Validation
            //Step 2: Logging
            connection.Open();
            var insertSql = "INSERT INTO dbo.Logs (Message) VALUES ('"+datetime+" / "+level+" / "+opr+" / "+cat+" / "+message+"')";
            var command = new SqlCommand(insertSql, connection);
            var rows = command.ExecuteNonQuery();
            connection.Close();
            if (rows == 1)
            {
                return Results.Pass;
            }
            return Results.Fail;
        }
        public Results Clear()
        {
            connection.Open();
            var insertSql = "DELETE FROM dbo.Logs";
            var command = new SqlCommand(insertSql, connection);
            var rows = command.ExecuteNonQuery();
            connection.Close();
            if (rows == 0)
            {
                return Results.Pass;
            }
            return Results.Fail;
        }
    }
    
}
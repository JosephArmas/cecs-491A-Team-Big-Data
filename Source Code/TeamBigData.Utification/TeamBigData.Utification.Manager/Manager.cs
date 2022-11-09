using System.Diagnostics;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Registration;
using TeamBigData.Utification.SQLDataAccess;
namespace TeamBigData.Utification.ManagerLayer
{
    public class Manager
    {
        public Response InsertUser(String email, String password)
        {
            var response = new Response();
            Stopwatch stopwatch = new Stopwatch();
            response.isSuccessful = false;
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBInserter sqlDAO = new SqlDAO(connectionString);
            var accountManager = new AccountManager(sqlDAO);
            stopwatch.Start();
            response = accountManager.InsertUser("dbo.Users", email, password).Result;
            stopwatch.Stop();
            String username = response.errorMessage.Substring(47);
            var logger = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            var insertSql = "INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Warning'," + email + ",'" + DateTime.UtcNow.ToString() + "', 'Manager.InsertUser()', 'Data',";
            if (stopwatch.ElapsedMilliseconds > 5000)
            {
                insertSql += "'Creating an a Account Took Longer than 5 Seconds')";
            }
            else if(response.isSuccessful)
            {
                insertSql += "'Account Created Successfully')";
            }
            logger.Execute(insertSql);
            return response;
        }
    }
}
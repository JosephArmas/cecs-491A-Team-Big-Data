using System.Diagnostics;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
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
            String insertSql;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                String username = response.errorMessage.Substring(47);
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    insertSql = "INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Warning', '" + email + "' ,'" + DateTime.UtcNow.ToString() + "', 'Manager.InsertUser()', 'Data', 'Account Registration Took Longer than 5 Seconds')";
                }
                else
                {
                    insertSql = "INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Info', '" + email + "' ,'" + DateTime.UtcNow.ToString() + "', 'Manager.InsertUser()', 'Data', 'Account Registration Successful')";
                }
            }
            else
            {
                insertSql = "INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Error', '" + email + "' ,'" + DateTime.UtcNow.ToString() + "', 'Manager.InsertUser()', 'Data', 'Error in Creating Account')";
            }
            var logRes = logger.Log(insertSql);
            return response;
        }
    }
}
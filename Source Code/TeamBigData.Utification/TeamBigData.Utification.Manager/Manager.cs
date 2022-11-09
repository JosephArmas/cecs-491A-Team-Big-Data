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
            if(stopwatch.ElapsedMilliseconds > 5000)
            {
                //TODO: Make System Log that it took longer than 5 seconds
            }
            return response;
        }
    }
}
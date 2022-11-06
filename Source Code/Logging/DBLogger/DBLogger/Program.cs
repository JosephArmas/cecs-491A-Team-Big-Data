using DataAccess;
using Domain;
using Logging.Implementations;

Console.WriteLine("Hello, World!");
var test = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User Id=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
var log = new Logger(test);
Result finale = log.Log("INSERT INTO dbo.Logs (Message) VALUES ('" + DateTime.UtcNow.ToString() + " / This is an automated test Info View')");
if (finale.IsSuccessful == false && finale.ErrorMessage != null)
{
    Console.WriteLine(finale.ErrorMessage);
}
Console.ReadLine();
// See https://aka.ms/new-console-template for more information
using TeamBigData.Utification.Registration;
using TeamBigData.Utification.SQLDataAccess;
User me = new User("daviddg", "password", "daviddg5@yahoo.com");
SQLDB s = new SQLDB();
Response r = s.InsertUser(me);
Console.WriteLine(r.ToString());
Console.ReadLine();

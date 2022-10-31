// See https://aka.ms/new-console-template for more information
using TeamBigData.Utification.Registration;
using TeamBigData.Utification.SQLDataAccess;
User me = new User("daviddg5", "password", "daviddg5@yahoo.com");
SQLDB s = new SQLDB();
Response r = s.InsertUser(me.GetUsername(), me.GetPassword(), me.GetEmail());
Console.WriteLine(r.ToString());
Console.ReadLine();

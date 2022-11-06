// See https://aka.ms/new-console-template for more information
using TeamBigData.Utification.Registration;
using TeamBigData.Utification.SQLDataAccess;

var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
SqlDAO testDBO = new SqlDAO(connectionString);
Registerer testRegister = new Registerer(testDBO);
Console.WriteLine(testDBO);
Console.WriteLine(testRegister);
String username = "daviddg5";
String password = "password";
String email = "daviddg5@yahoo.com";
String email2 = "daviddg@yahoo.com";
//Act
testDBO.Clear("dbo.TestUsers");
testRegister.InsertUser("dbo.TestUsers", username, password, email);
var actual = testRegister.InsertUser("dbo.TestUsers", username, password, email2);
//Assert
Console.WriteLine(actual.errorMessage);
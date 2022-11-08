//Arrange
using TeamBigData.Utification.Registration;
using TeamBigData.Utification.SQLDataAccess;

var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
SqlDAO testDBO = new SqlDAO(connectionString);
AccountManager testRegister = new AccountManager(testDBO);
String password = "password";
String email = "daviddg@yahoo.com";
//Act
await testDBO.Clear("dbo.TestUsers");
var actual = await testRegister.InsertUser("dbo.TestUsers", email, password);
Console.WriteLine(actual.errorMessage);
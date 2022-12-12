namespace AuthorizationTests
{
    [TestClass]
    public class AuthorizationIntegrationTests
    {
        [TestMethod]
        public void UserIsInRole(string user, string role)
        {
            Console.Write(user.IsInRole(role));
        }

        public void UserHasMultipleRoles(string user,string role)
        {
            Console.Write(user.Contains(role));
        }
    }
}
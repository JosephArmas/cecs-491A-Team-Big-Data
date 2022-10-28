using BigData.Utification.Registration;
using System.Diagnostics;

namespace BigData.Utification.Registration.Test
{
    [TestClass]
    public class RegistrationTests
    {

        [TestMethod]
        public void ShouldCreateUserObjectWithParameters()
        {
            //Arrange
            var expected = typeof(User);
            String username = "daviddg";
            String password = "password";
            String email = "david.degirolamo@student.csulb.edu";

            //Act
            User actual = new User(username, password, email);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected == actual.GetType());
            Assert.AreEqual(actual.GetUsername(), username);
            Assert.AreEqual(actual.GetPassword(), password);
            Assert.AreEqual(actual.GetEmail(), email);
        }

        [TestMethod]
        public void InvalidEmailFailure()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void InvalidPassphraseFailure()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            long expected = 5 * 60000;
            //TODO: Create Object to start registration process

            //Act
            stopwatch.Start();
            //TODO: Run Registration process

            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < expected);
        }

        [TestMethod]
        public void ShouldCreatePersistentUserInDB()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
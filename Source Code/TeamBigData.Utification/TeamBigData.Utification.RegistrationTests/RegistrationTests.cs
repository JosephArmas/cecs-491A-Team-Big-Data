using TeamBigData.Utification.Registration;
using System.Diagnostics;

namespace TeamBigData.Utification.Registration.Test
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
        public void ChecksInvalidUsernameCharacters()
        {
            //Arrange
            RegistrationServices testServices = new RegistrationServices();
            String username = "daviddg5!";
            //Act
            bool actual = testServices.IsValidUsername(username);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksTooShortUsername()
        {
            //Arrange
            RegistrationServices testServices = new RegistrationServices();
            String username = "davidd";
            //Act
            bool actual = testServices.IsValidUsername(username);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksInvalidPassphraseCharacters()
        {
            //Arrange
            RegistrationServices testServices = new RegistrationServices();
            String password = "pa$$w*rd";
            //Act
            bool actual = testServices.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksTooShortPassphrase()
        {
            //Arrange
            RegistrationServices testServices = new RegistrationServices();
            String password = "123";
            //Act
            bool actual = testServices.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksInvalidEmailCharacters()
        {
            //Arrange
            RegistrationServices testServices = new RegistrationServices();
            String email = "pa$$w*rd@yahoo.com";
            //Act
            bool actual = testServices.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksAtSignInEmail()
        {
            //Arrange
            RegistrationServices testServices = new RegistrationServices();
            String email = "email.email";
            //Act
            bool actual = testServices.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
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
    }
}
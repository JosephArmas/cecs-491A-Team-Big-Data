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
            String username = "daviddg5!";
            //Act
            bool actual = Registerer.IsValidUsername(username);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksTooShortUsername()
        {
            //Arrange
            String username = "davidd";
            //Act
            bool actual = Registerer.IsValidUsername(username);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksInvalidPassphraseCharacters()
        {
            //Arrange
            String password = "pa$$w*rd";
            //Act
            bool actual = Registerer.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksTooShortPassphrase()
        {
            //Arrange
            String password = "123";
            //Act
            bool actual = Registerer.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksInvalidEmailCharacters()
        {
            //Arrange
            String email = "pa$$w*rd@yahoo.com";
            //Act
            bool actual = Registerer.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksAtSignInEmail()
        {

            //Arrange
            String email = "email.email";
            //Act
            bool actual = Registerer.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }
    }
}
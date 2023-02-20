using TeamBigData.Utification.AccountServices;
using System.Diagnostics;
using System.Drawing.Printing;

namespace TeamBigData.Utification.Registration.Test
{
    [TestClass]
    public class RegistrationTests
    {

        [TestMethod]
        public void ChecksInvalidPassphraseCharacters()
        {
            //Arrange
            String password = "pa$$w*rd";
            //Act
            bool actual = AccountRegisterer.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AllowSpacesInPassword()
        {
            //Arrange
            String password = "pass word";
            //Act
            bool actual = AccountRegisterer.IsValidPassword(password);
            //Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ChecksTooShortPassphrase()
        {
            //Arrange
            String password = "123";
            //Act
            bool actual = AccountRegisterer.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksInvalidEmailCharacters()
        {
            //Arrange
            String email = "pa$$w*rd@yahoo.com";
            //Act
            bool actual = AccountRegisterer.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksAtSignInEmail()
        {

            //Arrange
            String email = "email.email";
            //Act
            bool actual = AccountRegisterer.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }
    }
}
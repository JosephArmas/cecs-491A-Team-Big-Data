using TeamBigData.Utification.AccountServices;
using System.Diagnostics;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.RegistrationTests
{
    [TestClass]
    public class RegistrationTests
    {

        [TestMethod]
        public void ChecksInvalidPassphraseCharacters()
        {
            string password = "pa$$w*rd";
            //Act
            bool actual = InputValidation.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AllowSpacesInPassword()
        {
            //Arrange
            InputValidation valid = new InputValidation();
            string password = "pass word";
            //Act
            bool actual = InputValidation.IsValidPassword(password);
            //Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ChecksTooShortPassphrase()
        {
            //Arrange
            InputValidation valid = new InputValidation();
            string password = "123";
            //Act
            bool actual = InputValidation.IsValidPassword(password);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksInvalidEmailCharacters()
        {
            //Arrange
            InputValidation valid = new InputValidation();
            string email = "pa$$w*rd@yahoo.com";
            //Act
            bool actual = InputValidation.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChecksAtSignInEmail()
        {

            //Arrange
            InputValidation valid = new InputValidation();
            string email = "email.email";
            //Act
            bool actual = InputValidation.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }
    }
}
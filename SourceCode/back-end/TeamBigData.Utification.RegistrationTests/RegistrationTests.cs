using TeamBigData.Utification.AccountServices;
using System.Diagnostics;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Registration.Test
{
    [TestClass]
    public class RegistrationTests
    {

        [TestMethod]
        public void ChecksInvalidPassphraseCharacters()
        {
            String password = "pa$$w*rd";
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
            String password = "pass word";
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
            String password = "123";
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
            String email = "pa$$w*rd@yahoo.com";
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
            String email = "email.email";
            //Act
            bool actual = InputValidation.IsValidEmail(email);
            //Assert
            Assert.IsFalse(actual);
        }
    }
}
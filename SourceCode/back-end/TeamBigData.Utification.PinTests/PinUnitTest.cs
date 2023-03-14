using TeamBigData.Utification.PinManagers;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;

namespace TeamBigData.Utification.PinTests
{
    [TestClass]
    public class PinUnitTest
    {
        [TestMethod]
        public void GetAllPinsFromDatabase()
        {
            //Arrange
            PinManager pinMan = new PinManager();
            UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32",false);
            //Act
            var result = pinMan.GetListOfAllPins(userAccount);
            //Assert
            Assert.IsTrue(result.Count > 0);
        }
        [TestMethod]
        public void GetAllPinsIsLogged()
        {
            //Arrange
            PinService pinSec = new PinService();
            UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32", false);
            //Act
            var result = pinSec.GetPinTable(userAccount);
            //Assert
            Assert.IsTrue(result.Result.Count > 0);
        }
        [TestMethod]
        public void PostNewPinIsLogged()
        {
            //Arrange
            PinManager pinMan = new PinManager();
            UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32", false);
            Pin pin = new Pin(8,1001,"33.797299272696115", "-118.11066677246092", 2, "`<h1>test junk</h1><p></p>`", 0);
            //Act
            var result = pinMan.SaveNewPin(pin, userAccount);
            //Assert
            Assert.IsTrue(result.isSuccessful);
        }
    }
}
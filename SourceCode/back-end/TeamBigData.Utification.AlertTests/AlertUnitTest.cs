using TeamBigData.Utification.AlertManagers;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Alerts;

namespace TeamBigData.Utification.AlertTests
{
    [TestClass]
    public class AlertUnitTest
    {
        [TestMethod]
        public async Task GetAllAlertsFromDatabase()
        {
            //Arrange
            AlertManager alertMan = new AlertManager();
            UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32", false);
            //Act
            var result = await alertMan.GetListOfAllAlerts(userAccount._userHash);
            //Assert
            Assert.IsTrue(result.data.Count>0);
        }
        /*[TestMethod]
        public async Task Get100AlertsFromDatabase()
        {
            //Arrange
            AlertManager alertMan = new AlertManager();
            UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32", false);
            //Act
            var result = await alertMan.GetAlertsAdded();
            //Assert
            Assert.IsTrue(result.Length > 0);
        }*/
        [TestMethod]
        public async Task Fails_UserEntersLongTitleAsync()
        {
            //Arrange
            AlertManager alertMan = new AlertManager();
            UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32", false);
            Alert alert = new Alert(1001, "","","fiftycharactersssssssssssssssssssssssssssssssssssssssssssssss","","");
            //Act
            var result = await alertMan.SaveNewAlert(alert,userAccount. ).ConfigureAwait(false);
            var test = alert._description;
            //Assert
            Assert.IsTrue(result == true);
            Assert.IsTrue(!string.IsNullOrEmpty(test));
            Assert.IsTrue(test.Length< 50);
        }
    }
}
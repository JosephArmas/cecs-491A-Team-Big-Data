using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;

namespace TeamBigData.Utification.PinServices
{
    public class PinService
    {
        public async Task<List<Pin>> GetPinTable(UserAccount userAccount)
        {
            var tcs = new TaskCompletionSource<List<Pin>>();
            List<Pin> pins = new List<Pin>();
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Features;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            IDBSelecter sqlSelect = new SqlDAO(connectionString);
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var result = await sqlSelect.SelectPinTable().ConfigureAwait(false);
            if (result.Count== 0)
            {
                log = new Log(1, "Error", userAccount._userHash, "PinService.GetPinTable()", "Data", "Error Select Pint Table returns empty.");
                logger.Log(log);
                tcs.SetResult(result);
                return tcs.Task.Result;
            }
            else
            {
                log = new Log(1, "Info", userAccount._userHash, "PinService.GetPinTable()", "Data", "Get pins table Successfully.");
                logger.Log(log);
            } 
            tcs.SetResult(result);
            return tcs.Task.Result;
        }
        public async Task<Response> StoreNewPin(Pin pin, UserAccount userAccount)
        {
            var tcs = new TaskCompletionSource<Response>();
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Features;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            IDBInserter sqlInsert = new SqlDAO(connectionString);
            Log log = new Log(1, "info", userAccount._userHash, "PinService.GetPinTable()", "Data", "Get pins table successfully.");
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var result = await sqlInsert.InsertPin(pin).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                log = new Log(1, "Error", userAccount._userHash, "PinService.StoreNewPin()", "Data", "Error Failed to Store New Pin.");
                logger.Log(log);
                tcs.SetResult(result);
                return tcs.Task.Result;
            }
            else
            {
                log = new Log(1, "Info", userAccount._userHash, "PinService.StoreNewPin()", "Data", "Store New Pin Successfully.");
                logger.Log(log);
            }
            tcs.SetResult(result);
            return tcs.Task.Result;
        }
    }
}
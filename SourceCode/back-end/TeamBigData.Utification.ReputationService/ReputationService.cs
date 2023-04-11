using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Logging;

namespace TeamBigData.Utification.ReputationServices
{
    public class ReputationService
    {
        private readonly Response _result;
        private readonly SqlDAO _reportsSqlDAO;
        private readonly SqlDAO _userAccountSqlDAO;
        private Report _report;
        private UserAccount _userAccount;
        private UserProfile _userProfile;
        private Logger _logger;
        public ReputationService(Response result, SqlDAO reportsSqlDao, SqlDAO userAccountSqlDAO, Report report, UserAccount userAccount, UserProfile userProfile, Logger logger)
        {
            _result = result;
            _reportsSqlDAO = reportsSqlDao;
            _userAccountSqlDAO = userAccountSqlDAO;
            _report = report; 
            _userAccount = userAccount; 
            _userProfile = userProfile;
            _logger = logger;
        }

        public async Task<Response> UpdateUserReputation()
        {
            var getReputation = await _reportsSqlDAO.SelectNewReputation(_report).ConfigureAwait(false);

            Console.WriteLine("Got Reputation");
            var getHash = _userAccountSqlDAO.SelectUserAccount(ref _userAccount, _userAccount._username);

            if (getReputation.isSuccessful)
            {
                _result.isSuccessful = true;
                var log = _logger.Log(new Log(1, "Info", _userAccount._userHash, "SelectNewReputation()", "dbo.UserProfile", "Successfully retrieved user reputation"));
                Console.WriteLine("Logging success case result: " + log.Result.isSuccessful.ToString());
                //var updateReputation
            }
            else
            {
                var log = _logger.Log(new Log(1, "Error", _userAccount._userHash, "SelectUserProfile()", "dbo.UserProfile", "Failed to retrieve user reputation"));
                Console.WriteLine("Logs failed case result: " + log.Result.isSuccessful.ToString());
            }
            return _result;
        }
                
        public async Task<Response> StoreNewReport()
        {
            Console.WriteLine("Accessing DAO");
            var insertReport = await _reportsSqlDAO.InsertUserReport(_report).ConfigureAwait(false);

            if(insertReport.isSuccessful) 
            {
                Console.WriteLine("Got passed the DAO");
                _result.isSuccessful = true;
            }

            return _result;
        }

    }
}
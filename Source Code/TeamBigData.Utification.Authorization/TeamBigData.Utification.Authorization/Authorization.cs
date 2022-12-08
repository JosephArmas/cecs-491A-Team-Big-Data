using System.Runtime.InteropServices.ComTypes;
using System.Security;
using TeamBigData.Utification.Authorization.Abstraction;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstraction;

namespace TeamBigData.Utification.Authorization
{
    public class Authorization : IAuthorization
    {
        private readonly IDBSelecter _dbSelect;
        public Authorization(IDBSelecter dbSelect)
        {
            _dbSelect = dbSelect;
        }
        public Response GetUserProfileTable(List<UserProfile> list)
        {
            Response result = new Response();
            var selectSql = "SELECT * FROM dbo.UserProfiles";
            //var command = new
            
            return result;
        }
    }
}
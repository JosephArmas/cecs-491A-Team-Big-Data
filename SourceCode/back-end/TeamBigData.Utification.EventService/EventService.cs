using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.EventService
{
    public class EventService
    {
        private readonly SqlDAO _dao;
        private readonly DBConnectionString connString = new DBConnectionString();

        /*
        public EventService(SqlDAO dao)
        {
            _dao = dao;
        }
        */

        
        public async Task<Response> CreateEvent(string title, string description)
        {
            var dao = new SqlDAO(connString._connectionStringFeatures);
            return await dao.InsertEvent(title, description);
        }


    }
    
}

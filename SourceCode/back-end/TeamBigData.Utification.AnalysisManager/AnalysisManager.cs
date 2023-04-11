using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.AnalysisManagers
{
    public class AnalysisManager
    {
        private readonly string _logConnectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
        public async Task<DataResponse<int[]>> GetLogins()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            var response = await analyzer.GetNewLogins();
            return response;
        }

        public async Task<DataResponse<int[]>> GetRegistrations()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            int[] rows = new int[91];
            var response = await analyzer.GetNewRegistrations();
            return response;
        }

        public async Task<DataResponse<int[]>> GetPinsAdded()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            int[] rows = new int[8];
            var response = await analyzer.GetPinsAdded();
            return response;
        }

        public async Task<DataResponse<int[]>> GetPinPulls()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            int[] rows = new int[31];
            var response = await analyzer.GetPinsAdded();
            return response;
        }
    }
}
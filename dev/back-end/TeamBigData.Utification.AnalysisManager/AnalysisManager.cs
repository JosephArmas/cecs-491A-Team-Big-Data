using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.AnalysisManagers
{
    public class AnalysisManager
    {
        private readonly string _logConnectionString = @"Server=localhost,1433;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
        public async Task<int[]> GetLogins()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            int[] rows = new int[91];
            var response = await analyzer.GetNewLogins(ref rows);
            return rows;
        }

        public async Task<int[]> GetRegistrations()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            int[] rows = new int[91];
            var response = await analyzer.GetNewRegistrations(ref rows);
            return rows;
        }

        public async Task<int[]> GetPinsAdded()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            int[] rows = new int[8];
            var response = await analyzer.GetPinsAdded(ref rows);
            return rows;
        }

        public async Task<int[]> GetPinPulls()
        {
            IDBAnalysis analyzer = new SqlDAO(_logConnectionString);
            int[] rows = new int[31];
            var response = await analyzer.GetPinsAdded(ref rows);
            return rows;
        }
    }
}
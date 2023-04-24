using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.AnalysisManagers
{
    public class AnalysisManager
    {
        private readonly IDBAnalysis analyzer;
        public AnalysisManager(SqlDAO DAO) 
        { 
            analyzer = DAO;
        }

        public async Task<DataResponse<int[]>> GetLogins()
        {
            var response = await analyzer.GetNewLogins();
            return response;
        }

        public async Task<DataResponse<int[]>> GetRegistrations()
        {
            int[] rows = new int[91];
            var response = await analyzer.GetNewRegistrations();
            return response;
        }

        public async Task<DataResponse<int[]>> GetPinsAdded()
        {
            int[] rows = new int[8];
            var response = await analyzer.GetPinsAdded();
            return response;
        }

        public async Task<DataResponse<int[]>> GetPinPulls()
        {
            int[] rows = new int[31];
            var response = await analyzer.GetPinsAdded();
            return response;
        }
    }
}
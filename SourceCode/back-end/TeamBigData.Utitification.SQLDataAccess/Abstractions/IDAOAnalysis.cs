using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDAOAnalysis
    {
        public Task<Response> GetNewLogins(ref List<AnalysisRow> rows);
        public Task<Response> GetNewRegistrations(ref List<AnalysisRow> rows);
        public Task<Response> GetNewPins(ref List<AnalysisRow> rows);
        public Task<Response> GetNewEvents(ref List<AnalysisRow> rows);

    }
}

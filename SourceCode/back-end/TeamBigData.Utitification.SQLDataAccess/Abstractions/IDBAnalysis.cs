using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBAnalysis
    {
        public Task<DataResponse<int[]>> GetNewLogins();
        public Task<DataResponse<int[]>> GetNewRegistrations();
        public Task<DataResponse<int[]>> GetPinsAdded();
        public Task<DataResponse<int[]>> GetPinPulls();

    }
}

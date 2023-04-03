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
        public Task<Response> GetNewLogins(ref int[] rows);
        public Task<Response> GetNewRegistrations(ref int[] rows);
        public Task<Response> GetPinsAdded(ref int[] rows);
        public Task<Response> GetPinPulls(ref int[] rows);

    }
}

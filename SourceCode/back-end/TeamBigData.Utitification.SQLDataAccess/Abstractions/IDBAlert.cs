using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBAlert
    {
        public Task<Response> GetAlertsAdded(ref int[] rows);
    }
}

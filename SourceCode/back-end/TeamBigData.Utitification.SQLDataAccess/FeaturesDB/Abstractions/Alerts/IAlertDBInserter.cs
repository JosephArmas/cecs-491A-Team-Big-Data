using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Alerts
{
    public interface IAlertDBInserter
    {
        public Task<Response> InsertAlert(Alert alert);
    }
}
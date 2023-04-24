using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins
{
    public interface IPinDBInserter
    {
        public Task<Response> InsertNewPin(Pin pin);
    }
}

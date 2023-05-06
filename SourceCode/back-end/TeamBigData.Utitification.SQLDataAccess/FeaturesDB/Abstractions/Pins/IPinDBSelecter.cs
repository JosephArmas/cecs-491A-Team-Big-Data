using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.DTO;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins
{
    public interface IPinDBSelecter
    {
        public Task<DataResponse<List<PinResponse>>> SelectPinTable();
<<<<<<< HEAD
        public Task<DataResponse<List<PinResponse>>> SelectEnabledPins();
=======
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
    }
}

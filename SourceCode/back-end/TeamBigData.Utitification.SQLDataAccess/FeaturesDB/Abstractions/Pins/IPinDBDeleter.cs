using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins
{
    public interface IPinDBDeleter
    {
        public Task<Response> DeletePinsLinkedToUser(int userID);
        public Task<Response> DeletePinFromTable(int pinID);
    }
}

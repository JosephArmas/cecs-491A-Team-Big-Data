using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins
{
    public interface IPinDBUpdater
    {
        public Task<Response> UpdatePinContent(int pinID, int userID, String description);
        public Task<Response> UpdatePinType(int pinID, int userID, int pinType);
        public Task<Response> UpdatePinToDisabled(int pinID, int userID);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices
{
    public interface IServicesDBInserter
    {
        public Task<Response> InsertProvider(ServiceModel serv);

        public Task<Response> InsertServiceReq(ServiceModel serv, Pin pin);
    }
}

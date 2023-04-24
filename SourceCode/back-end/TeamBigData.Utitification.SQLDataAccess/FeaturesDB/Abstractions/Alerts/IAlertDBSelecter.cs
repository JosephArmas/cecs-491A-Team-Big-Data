﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Alerts
{
    public interface IAlertDBSelecter
    {
        public Task<DataResponse<List<Alert>>> SelectAlertTable();
    }
}

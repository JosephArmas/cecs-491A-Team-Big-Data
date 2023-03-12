﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.DeletionService
{
    public interface IDeletionService
    {
        Task<Response> DeletePIIFeatures();

        Task<Response> DeletePIIProfile();
    }
}
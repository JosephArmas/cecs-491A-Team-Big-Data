using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Authorization.Abstraction
{
    public interface IAuthorization
    {
        public Response GetUserProfileTable(List<UserProfile> list);
    }
}

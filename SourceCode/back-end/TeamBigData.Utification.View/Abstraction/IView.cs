using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.View.Abstraction
{
    public interface IView
    {
        public Response DisplayMenu(ref UserProfile userProfile, ref String userhash);
    }
}

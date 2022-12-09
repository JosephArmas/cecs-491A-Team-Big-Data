using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.AuthZ.Abstraction
{
    public interface IMenu
    {
        public bool DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile);
    }
}

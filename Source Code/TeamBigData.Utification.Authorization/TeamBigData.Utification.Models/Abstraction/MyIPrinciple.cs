using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models.Abstraction
{
    interface MyIPrincipal : IPrincipal
    {
        string Id { get; set; }
        string UserName { get; set; }
    }
}

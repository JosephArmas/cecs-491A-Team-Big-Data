using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Cross
{
    public enum Results
    {
        Unknown = 0,
        Pass = 1,
        Fail =2,
    }
    public enum Loglevel
    {
        Info = 0,
        Debug = 1,
        Warning = 2,
        Error = 3,
    }
    public enum Category
    {
        View = 0,
        Business = 1,
        Server = 2,
        Data = 3,
        DataStore = 4,
    }
}

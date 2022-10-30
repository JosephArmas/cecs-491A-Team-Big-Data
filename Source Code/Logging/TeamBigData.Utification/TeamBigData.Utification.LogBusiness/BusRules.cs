using TeamBigData.Utification.LogServices;
using TeamBigData.Utification.Cross;
namespace TeamBigData.Utification.LogBusiness
{
    public class BusRules
    {
        private readonly Service serv = new Service();
        public BusRules()
        {
            var serv = new Service();
        }
        public Results BusLog(string datetime, Loglevel level, string opr, Category cat, string message)
        {
            //errored out, need to intialize
            return serv.ServiceLog(datetime, level, opr, cat, message);
        }
        public Results BusDel()
        {
            return serv.ServDel();
        }
    }
}
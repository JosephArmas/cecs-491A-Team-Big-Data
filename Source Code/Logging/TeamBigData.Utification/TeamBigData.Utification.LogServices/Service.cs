using TeamBigData.Utification.DALogging;
using TeamBigData.Utification.Cross;
namespace TeamBigData.Utification.LogServices
{
    public class Service
    {
        private readonly DAL dataacc = new DAL();
        public Service()
        {
            var dataacc = new DAL();
        }
        public Results ServiceLog(string datetime, Loglevel level, string opr, Category cat, string message)
        {
            return dataacc.Log(datetime,level,opr,cat,message);
        }
        public Results ServDel()
        {
            return dataacc.Clear();
        }
    }
}
using DataAccess.Interfaces;
using Database;

namespace DataAccess.Repositories
{
    public class PriceSubscribersRep : IPriceSubscribersRep
    {
        private readonly MasterContext _masterContext;

        public PriceSubscribersRep(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }
    }
}
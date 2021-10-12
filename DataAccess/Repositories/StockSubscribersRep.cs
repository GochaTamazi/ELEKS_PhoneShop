using DataAccess.Interfaces;
using Database;

namespace DataAccess.Repositories
{
    public class StockSubscribersRep : IStockSubscribersRep
    {
        private readonly MasterContext _masterContext;

        public StockSubscribersRep(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }
    }
}
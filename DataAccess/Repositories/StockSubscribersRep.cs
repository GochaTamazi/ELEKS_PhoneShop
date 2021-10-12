using DataAccess.Interfaces;
using Database;
using Models.Entities.PhoneShop;

namespace DataAccess.Repositories
{
    public class StockSubscribersRep : IStockSubscribersRep
    {
        private readonly MasterContext _masterContext;
        private readonly IGenericRep<StockSubscriber> _genStockSubscriber;

        public StockSubscribersRep(MasterContext masterContext, IGenericRep<StockSubscriber> genStockSubscriber)
        {
            _masterContext = masterContext;
            _genStockSubscriber = genStockSubscriber;
        }
    }
}
using DataAccess.Interfaces;
using Database;
using Models.Entities.PhoneShop;

namespace DataAccess.Repositories
{
    public class PriceSubscribersRep : IPriceSubscribersRep
    {
        private readonly MasterContext _masterContext;
        private readonly IGenericRep<PriceSubscriber> _genPriceSubscriber;

        public PriceSubscribersRep(MasterContext masterContext, IGenericRep<PriceSubscriber> genPriceSubscriber)
        {
            _masterContext = masterContext;
            _genPriceSubscriber = genPriceSubscriber;
        }
    }
}
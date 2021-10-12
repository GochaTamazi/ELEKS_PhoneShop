using DataAccess.Interfaces;
using Database;
using Models.Entities.PhoneShop;

namespace DataAccess.Repositories
{
    public class PhonesPhoneShopRep: IPhonesPhoneShopRep 
    {
        private readonly MasterContext _masterContext;
        private readonly IGenericRep<Phone> _genPhonePhoneShop;

        public PhonesPhoneShopRep(MasterContext masterContext, IGenericRep<Phone> genPhonePhoneShop)
        {
            _masterContext = masterContext;
            _genPhonePhoneShop = genPhonePhoneShop;
        }
    }
}
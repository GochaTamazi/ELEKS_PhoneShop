using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend.Forms;
using Application.Interfaces;
using AutoMapper;
using DataAccess.Interfaces;
using Database.Models;

namespace Application.Services
{
    public class Subscribers : ISubscribers
    {
        private readonly IGeneralRepository<PriceSubscriber> _priceSubscribersRepository;
        private readonly IGeneralRepository<StockSubscriber> _stockSubscribersRepository;
        private readonly IMapper _mapper;

        public Subscribers(
            IGeneralRepository<PriceSubscriber> priceSubscribersRepository,
            IGeneralRepository<StockSubscriber> stockSubscribersRepository,
            IMapper mapper
        )
        {
            _priceSubscribersRepository = priceSubscribersRepository;
            _stockSubscribersRepository = stockSubscribersRepository;
            _mapper = mapper;
        }

        public async Task SubscribeOnPriceAsync(PriceSubscriberForm priceSubscriberForm, CancellationToken token)
        {
            var priceSubscriber = _mapper.Map<PriceSubscriber>(priceSubscriberForm);

            await _priceSubscribersRepository.InsertIfNotExistAsync(s =>
                    s.BrandSlug == priceSubscriber.BrandSlug &&
                    s.PhoneSlug == priceSubscriber.PhoneSlug &&
                    s.Email == priceSubscriber.Email,
                priceSubscriber, token);
        }

        public async Task SubscribeOnStockAsync(StockSubscriberForm stockSubscriberForm, CancellationToken token)
        {
            var stockSubscriber = _mapper.Map<StockSubscriber>(stockSubscriberForm);

            await _stockSubscribersRepository.InsertIfNotExistAsync(s =>
                    s.BrandSlug == stockSubscriber.BrandSlug &&
                    s.PhoneSlug == stockSubscriber.PhoneSlug &&
                    s.Email == stockSubscriber.Email,
                stockSubscriber, token);
        }
    }
}
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

        public async Task SubscribeOnPriceAsync(SubscriberForm subscriberForm, CancellationToken token)
        {
            var priceSubscriber = _mapper.Map<PriceSubscriber>(subscriberForm);

            await _priceSubscribersRepository.AddIfNotExistAsync(s =>
                    s.BrandSlug == priceSubscriber.BrandSlug &&
                    s.PhoneSlug == priceSubscriber.PhoneSlug &&
                    s.Email == priceSubscriber.Email,
                priceSubscriber, token);
        }

        public async Task SubscribeOnStockAsync(SubscriberForm subscriberForm, CancellationToken token)
        {
            var stockSubscriber = _mapper.Map<StockSubscriber>(subscriberForm);

            await _stockSubscribersRepository.AddIfNotExistAsync(s =>
                    s.BrandSlug == stockSubscriber.BrandSlug &&
                    s.PhoneSlug == stockSubscriber.PhoneSlug &&
                    s.Email == stockSubscriber.Email,
                stockSubscriber, token);
        }
    }
}
using Application.DTO.Frontend.Forms;
using Application.DTO.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.Interfaces;
using AutoMapper;
using Database.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Application.DTO.Frontend;

namespace Application.Services
{
    public class MapperProvider : IMapperProvider
    {
        private readonly IMapper _mapper;
        public IMapper GetMapper() => _mapper;

        public MapperProvider()
        {
            _mapper = Initialize();
        }

        private IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<BrandDto, Brand>()
                        .ForMember(x => x.Slug,
                            m => m.MapFrom(x => x.Brand_slug))
                        .ForMember(x => x.Name,
                            m => m.MapFrom(x => x.Brand_name))
                        .ForAllOtherMembers(m => m.Ignore());

                    cfg.CreateMap<PhoneSpecificationsDto, Phone>()
                        .ForMember(x => x.PhoneName, m => m.MapFrom(x => x.Data.Phone_name))
                        .ForMember(x => x.Dimension, m => m.MapFrom(x => x.Data.Dimension))
                        .ForMember(x => x.Os, m => m.MapFrom(x => x.Data.Os))
                        .ForMember(x => x.Storage, m => m.MapFrom(x => x.Data.Storage))
                        .ForMember(x => x.Thumbnail, m => m.MapFrom(x => x.Data.Thumbnail))
                        .ForMember(x => x.ReleaseDate, m => m.MapFrom(x => x.Data.Release_date))
                        .ForMember(x => x.Images,
                            m => m.MapFrom(x =>
                                JsonConvert.SerializeObject(x.Data.Phone_images, Formatting.Indented)))
                        .ForMember(x => x.Specifications,
                            m => m.MapFrom(x =>
                                JsonConvert.SerializeObject(x.Data.Specifications, Formatting.None)))
                        .ForAllOtherMembers(m => m.Ignore());

                    cfg.CreateMap<PhoneSpecFront, Phone>()
                        .ForMember(x => x.BrandSlug, m => m.MapFrom(x => x.BrandSlug))
                        .ForMember(x => x.PhoneSlug, m => m.MapFrom(x => x.PhoneSlug))
                        .ForMember(x => x.Price, m => m.MapFrom(x => x.Price))
                        .ForMember(x => x.Stock, m => m.MapFrom(x => x.Stock))
                        .ForMember(x => x.Hided, m => m.MapFrom(x => x.Hided))
                        .ForAllOtherMembers(m => m.Ignore());
                    
                    cfg.CreateMap<Phone, PhoneSpecFront>()
                        .ForMember(x => x.BrandSlug, m => m.MapFrom(x => x.BrandSlug))
                        .ForMember(x => x.PhoneSlug, m => m.MapFrom(x => x.PhoneSlug))
                        .ForMember(x => x.Price, m => m.MapFrom(x => x.Price))
                        .ForMember(x => x.Stock, m => m.MapFrom(x => x.Stock))
                        .ForMember(x => x.Hided, m => m.MapFrom(x => x.Hided))
                        .ForAllOtherMembers(m => m.Ignore());

                    cfg.CreateMap<Phone, Application.DTO.Frontend.PhoneDto>()
                        .ForMember(x => x.Id, m => m.MapFrom(x => x.Id))
                        .ForMember(x => x.BrandSlug, m => m.MapFrom(x => x.BrandSlug))
                        .ForMember(x => x.PhoneSlug, m => m.MapFrom(x => x.PhoneSlug))
                        .ForMember(x => x.PhoneName, m => m.MapFrom(x => x.PhoneName))
                        .ForMember(x => x.Dimension, m => m.MapFrom(x => x.Dimension))
                        .ForMember(x => x.Os, m => m.MapFrom(x => x.Os))
                        .ForMember(x => x.Storage, m => m.MapFrom(x => x.Storage))
                        .ForMember(x => x.Thumbnail, m => m.MapFrom(x => x.Thumbnail))
                        .ForMember(x => x.ReleaseDate, m => m.MapFrom(x => x.ReleaseDate))
                        .ForMember(x => x.Price, m => m.MapFrom(x => x.Price))
                        .ForMember(x => x.Stock, m => m.MapFrom(x => x.Stock))
                        .ForMember(x => x.Hided, m => m.MapFrom(x => x.Hided))
                        .ForMember(x => x.Images,
                            m => m.MapFrom(x =>
                                JsonConvert.DeserializeObject<List<string>>(x.Images)))
                        .ForMember(x => x.Specifications,
                            m => m.MapFrom(x =>
                                JsonConvert.DeserializeObject<List<SpecificationDto>>(x.Specifications)))
                        .ForAllOtherMembers(m => m.Ignore());

                    cfg.CreateMap<PriceSubscriberForm, PriceSubscriber>()
                        .ForMember(x => x.BrandSlug, m => m.MapFrom(x => x.BrandSlug))
                        .ForMember(x => x.PhoneSlug, m => m.MapFrom(x => x.PhoneSlug))
                        .ForMember(x => x.Email, m => m.MapFrom(x => x.Email))
                        .ForAllOtherMembers(m => m.Ignore());

                    cfg.CreateMap<StockSubscriberForm, StockSubscriber>()
                        .ForMember(x => x.BrandSlug, m => m.MapFrom(x => x.BrandSlug))
                        .ForMember(x => x.PhoneSlug, m => m.MapFrom(x => x.PhoneSlug))
                        .ForMember(x => x.Email, m => m.MapFrom(x => x.Email))
                        .ForAllOtherMembers(m => m.Ignore());
                }
            );
            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }
    }
}
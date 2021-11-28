using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.Interfaces;
using AutoMapper;
using DataAccess.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Application.Services
{
    public class CustomerPhones : ICustomerPhones
    {
        private readonly IGeneralRepository<Phone> _phonesRepository;
        private readonly IGeneralRepository<Comment> _commentsRepository;
        private readonly IMapper _mapper;

        public CustomerPhones(
            IGeneralRepository<Phone> phonesRepository,
            IGeneralRepository<Comment> commentsRepository,
            IMapper mapper
        )
        {
            _phonesRepository = phonesRepository;
            _commentsRepository = commentsRepository;
            _mapper = mapper;
        }

        public async Task<PhoneDto> GetOneAsync(string phoneSlug, CancellationToken token)
        {
            Expression<Func<Phone, bool>> phoneCondition = (phone) =>
                phone.PhoneSlug == phoneSlug &&
                phone.Hided == false;

            var phoneModel = await _phonesRepository.GetOneAsync(phoneCondition, token);
            if (phoneModel == null)
            {
                return null;
            }

            var phoneDto = _mapper.Map<PhoneDto>(phoneModel);

            Expression<Func<Comment, bool>> commentCondition = (comment) => comment.PhoneSlug == phoneSlug;

            var averageRating = await _commentsRepository.AverageAsync(commentCondition, phone => phone.Rating, token);

            if (averageRating != null)
            {
                phoneDto.AverageRating = Math.Round((double) averageRating, 1);
            }

            return phoneDto;
        }

        public async Task<PhonesPageFront> GetAllAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token)
        {
            Expression<Func<Phone, bool>> condition = (phone) =>
                EF.Functions.Like(phone.BrandSlug, $"%{filterForm.BrandName}%") &&
                EF.Functions.Like(phone.PhoneName, $"%{filterForm.PhoneName}%") &&
                filterForm.PriceMin <= phone.Price && phone.Price <= filterForm.PriceMax &&
                ((!filterForm.InStock) || 1 <= phone.Stock) &&
                phone.Hided == false;

            Expression<Func<Phone, object>> orderBy = filterForm.OrderBy switch
            {
                "PhoneName" => (phone) => phone.PhoneName,
                "BrandSlug" => (phone) => phone.BrandSlug,
                "Price" => (phone) => phone.Price,
                "Stock" => (phone) => phone.Stock,
                _ => (phone) => phone.PhoneName
            };

            var phones = await _phonesRepository.GetAllAsync(condition, orderBy, token);

            var totalPages = (int) Math.Ceiling((double) phones.Count / pageSize);

            if (page <= 0)
            {
                page = 1;
            }

            return new PhonesPageFront()
            {
                TotalPhones = phones.Count,
                TotalPages = totalPages,
                PageSize = pageSize,
                Page = page,
                Phones = phones.ToPagedList(page, pageSize).ToList()
            };
        }
    }
}
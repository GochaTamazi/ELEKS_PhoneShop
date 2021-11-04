using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.Interfaces;
using DataAccess.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Application.Services
{
    public class CustomerPhones : ICustomerPhones
    {
        private readonly IGeneralRepository<Comment> _commentsRepository;
        private readonly IGeneralRepository<Phone> _phonesRepository;
        private readonly IGeneralRepository<PriceSubscriber> _priceSubscribersRepository;
        private readonly IGeneralRepository<StockSubscriber> _stockSubscribersRepository;
        private readonly IGeneralRepository<User> _usersRepository;
        private readonly IGeneralRepository<WishList> _wishListRepository;
        private readonly IMapperProvider _mapperProvider;

        public CustomerPhones(
            IGeneralRepository<Comment> commentsRepository,
            IGeneralRepository<Phone> phonesRepository,
            IGeneralRepository<PriceSubscriber> priceSubscribersRepository,
            IGeneralRepository<StockSubscriber> stockSubscribersRepository,
            IGeneralRepository<User> usersRepository,
            IGeneralRepository<WishList> wishListRepository,
            IMapperProvider mapperProvider
        )
        {
            _commentsRepository = commentsRepository;
            _phonesRepository = phonesRepository;
            _priceSubscribersRepository = priceSubscribersRepository;
            _stockSubscribersRepository = stockSubscribersRepository;
            _usersRepository = usersRepository;
            _wishListRepository = wishListRepository;
            _mapperProvider = mapperProvider;
        }

        public async Task AddToWishListAsync(string phoneSlug, string userMail,
            CancellationToken token)
        {
            var phone = await _phonesRepository.GetOneAsync(phone =>
                    phone.PhoneSlug == phoneSlug &&
                    phone.Hided != true,
                token);

            if (phone != null)
            {
                var user = await _usersRepository.GetOneAsync(user =>
                        user.Email == userMail,
                    token);

                if (user != null)
                {
                    var wishList = await _wishListRepository.GetOneAsync(list =>
                            list.UserId == user.Id &&
                            list.PhoneId == phone.Id,
                        token);

                    if (wishList == null)
                    {
                        var wishListNew = new WishList()
                        {
                            UserId = user.Id,
                            PhoneId = phone.Id
                        };

                        await _wishListRepository.InsertAsync(wishListNew, token);
                    }
                }
            }
        }

        public async Task RemoveFromWishListAsync(string phoneSlug, string userMail,
            CancellationToken token)
        {
            var phone = await _phonesRepository.GetOneAsync(phone =>
                    phone.PhoneSlug == phoneSlug &&
                    phone.Hided != true,
                token);

            if (phone != null)
            {
                var user = await _usersRepository.GetOneAsync(user =>
                        user.Email == userMail,
                    token);

                if (user != null)
                {
                    var wishList = await _wishListRepository.GetOneAsync(list =>
                            list.UserId == user.Id &&
                            list.PhoneId == phone.Id,
                        token);

                    if (wishList != null)
                    {
                        await _wishListRepository.RemoveAsync(wishList, token);
                    }
                }
            }
        }

        public async Task SubscribePriceAsync(PriceSubscriberForm priceSubscriberForm,
            CancellationToken token)
        {
            var priceSubscriber = _mapperProvider.GetMapper().Map<PriceSubscriber>(priceSubscriberForm);

            var priceSubscriberModel = await _priceSubscribersRepository.GetOneAsync(sub =>
                    sub.BrandSlug == priceSubscriber.BrandSlug &&
                    sub.PhoneSlug == priceSubscriber.PhoneSlug &&
                    sub.Email == priceSubscriber.Email,
                token);

            if (priceSubscriberModel == null)
            {
                await _priceSubscribersRepository.InsertAsync(priceSubscriber, token);
            }
        }

        public async Task SubscribeStockAsync(StockSubscriberForm stockSubscriberForm,
            CancellationToken token)
        {
            var stockSubscriber = _mapperProvider.GetMapper().Map<StockSubscriber>(stockSubscriberForm);

            var stockSubscriberModel = await _stockSubscribersRepository.GetOneAsync(sub =>
                    sub.BrandSlug == stockSubscriber.BrandSlug &&
                    sub.PhoneSlug == stockSubscriber.PhoneSlug &&
                    sub.Email == stockSubscriber.Email,
                token);

            if (stockSubscriberModel == null)
            {
                await _stockSubscribersRepository.InsertAsync(stockSubscriber, token);
            }
        }

        public async Task<CommentsPage> GetPhoneCommentsAsync(string phoneSlug, int page, int pageSize,
            CancellationToken token)
        {
            if (page <= 0)
            {
                page = 1;
            }

            Expression<Func<Comment, bool>> commentCondition = (comment) => comment.PhoneSlug == phoneSlug;

            var comments = await _commentsRepository.GetAllIncludeAsync(commentCondition,
                comment => comment.User, token);

            return new CommentsPage()
            {
                PhoneSlug = phoneSlug,
                TotalComments = comments.Count,
                TotalPages = (int) Math.Ceiling((double) comments.Count / pageSize),
                PageSize = pageSize,
                Page = page,
                Comments = comments.ToPagedList(page, pageSize).ToList()
            };
        }

        public async Task<List<WishList>> ShowWishListAsync(string userMail,
            CancellationToken token)
        {
            var user = await _usersRepository.GetOneAsync(user => user.Email == userMail, token);
            if (user != null)
            {
                return await _wishListRepository.GetAllIncludeAsync(
                    list => list.UserId == user.Id,
                    list => list.Phone,
                    token);
            }

            return new List<WishList>();
        }

        public async Task<DTO.Frontend.PhoneDto> GetPhoneAsync(string phoneSlug,
            CancellationToken token)
        {
            Expression<Func<Phone, bool>> phoneCondition = (phone) =>
                phone.PhoneSlug == phoneSlug &&
                phone.Hided == false;

            var phoneModel = await _phonesRepository.GetOneAsync(phoneCondition, token);
            if (phoneModel == null)
            {
                return new PhoneDto();
            }

            var phoneDto = _mapperProvider.GetMapper().Map<PhoneDto>(phoneModel);

            Expression<Func<Comment, bool>> commentCondition = (comment) =>
                comment.PhoneSlug == phoneSlug;

            var averageRating = await _commentsRepository.AverageAsync(commentCondition,
                phone => phone.Rating, token);

            if (averageRating != null)
            {
                phoneDto.AverageRating = Math.Round((double) averageRating, 1);
            }

            return phoneDto;
        }

        public async Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token)
        {
            Expression<Func<Phone, bool>> condition = (phone) =>
                EF.Functions.Like(phone.BrandSlug, $"%{filterForm.BrandName}%") &&
                EF.Functions.Like(phone.PhoneName, $"%{filterForm.PhoneName}%") &&
                filterForm.PriceMin <= phone.Price && phone.Price <= filterForm.PriceMax &&
                ((!filterForm.InStock) || 1 <= phone.Stock) &&
                phone.Hided == false;

            Expression<Func<Phone, object>> orderBy;
            switch (filterForm.OrderBy)
            {
                default:
                    orderBy = (phone) => phone.PhoneName;
                    break;
                case "PhoneName":
                    orderBy = (phone) => phone.PhoneName;
                    break;
                case "BrandSlug":
                    orderBy = (phone) => phone.BrandSlug;
                    break;
                case "Price":
                    orderBy = (phone) => phone.Price;
                    break;
                case "Stock":
                    orderBy = (phone) => phone.Stock;
                    break;
            }

            var phones = await _phonesRepository.GetAllAsync(
                condition,
                orderBy,
                token);

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

        public async Task<bool> PostCommentAsync(CommentForm commentForm,
            CancellationToken token)
        {
            var user = await _usersRepository.GetOneAsync(user => user.Email == commentForm.UserMail, token);
            if (user == null)
            {
                return false;
            }

            var commentNew = new Comment()
            {
                Comments = commentForm.Comments,
                Rating = commentForm.Rating,
                CreateTime = commentForm.CreateTime,
                PhoneSlug = commentForm.PhoneSlug,
                UserId = user.Id
            };

            await _commentsRepository.InsertOrUpdateAsync(comment =>
                comment.UserId == commentNew.UserId &&
                comment.PhoneSlug == commentNew.PhoneSlug, commentNew, token);

            return true;
        }
    }
}
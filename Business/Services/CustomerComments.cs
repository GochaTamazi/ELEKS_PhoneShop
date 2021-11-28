using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.Interfaces;
using AutoMapper;
using DataAccess.Interfaces;
using Database.Models;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Application.Services
{
    public class CustomerComments : ICustomerComments
    {
        private readonly IGeneralRepository<User> _usersRepository;
        private readonly IGeneralRepository<Comment> _commentsRepository;
        private readonly IMapper _mapper;

        public CustomerComments(
            IGeneralRepository<User> usersRepository,
            IGeneralRepository<Comment> commentsRepository,
            IMapper mapper
        )
        {
            _usersRepository = usersRepository;
            _commentsRepository = commentsRepository;
            _mapper = mapper;
        }

        public async Task<CommentsPage> GetAllAsync(string phoneSlug, int page, int pageSize, CancellationToken token)
        {
            var comments = await _commentsRepository.GetAllIncludeAsync(
                comment => comment.PhoneSlug == phoneSlug, comment => comment.User, token) ?? new List<Comment>();

            if (page < 1)
            {
                page = 1;
            }

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

        public async Task<bool> AddOrUpdateAsync(CommentForm commentForm, CancellationToken token)
        {
            var user = await _usersRepository.GetOneAsync(user => user.Email == commentForm.UserMail, token);
            if (user == null)
            {
                return false;
            }

            var comment = _mapper.Map<Comment>(commentForm);
            comment.UserId = user.Id;

            await _commentsRepository.AddOrUpdateAsync(
                c => c.UserId == comment.UserId && c.PhoneSlug == comment.PhoneSlug,
                comment, token);

            return true;
        }
    }
}
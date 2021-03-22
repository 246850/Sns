using AutoMapper;
using Calamus.AspNetCore.Users;
using Calamus.Infrastructure.Models;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calamus.Data;

namespace Sns.Services
{
    public class CommentService : ICommentService
    {
        private readonly SnsdbContext _snsdbContext;
        private readonly IMapper _mapper;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        public CommentService(SnsdbContext snsdbContext,
             IMapper mapper, IIdentityUser<int, LoginAuthModel> identityUser)
        {
            _snsdbContext = snsdbContext;
            _mapper = mapper;
            _identityUser = identityUser;
        }
        public void Create(CommentCreateRequestDTO request)
        {
            var entity = _mapper.Map<Comment>(request);
            entity.AccountId = _identityUser.Id;
            _snsdbContext.Comments.Add(entity);
            _snsdbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _snsdbContext.RemoveRange<Comment>(x => x.Id == id && x.AccountId == _identityUser.Id); // 移除评论
            _snsdbContext.RemoveRange<CommentThumb>(x => x.CommentId == id);    // 移除点赞
            _snsdbContext.SaveChanges();
        }

        public void ThumbupCreateOrRemove(int id)
        {
            if (_snsdbContext.Comments.Any(x => x.Id == id && x.AccountId == _identityUser.Id)) throw new CodeException("不能点赞自己");

            var entity = _snsdbContext.CommentThumbs.FirstOrDefault(x => x.AccountId == _identityUser.Id && x.CommentId == id);
            if (entity != null)
            {
                _snsdbContext.CommentThumbs.Remove(entity);
            }
            else
            {
                entity = new CommentThumb
                {
                    AccountId = _identityUser.Id,
                    CommentId = id
                };
                _snsdbContext.CommentThumbs.Add(entity);
            }
            _snsdbContext.SaveChanges();
        }
    }
}

using Calamus.AspNetCore.Users;
using Calamus.Infrastructure.Models;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sns.Services
{
    public class ThumbService : IThumbService
    {
        private readonly SnsdbContext _snsdbContext;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        public ThumbService(SnsdbContext snsdbContext, IIdentityUser<int, LoginAuthModel> identityUser)
        {
            _snsdbContext = snsdbContext;
            _identityUser = identityUser;
        }
        public void CreateOrRemove(int articleId)
        {
            if (_snsdbContext.Articles.Any(x => x.Id == articleId && x.AccountId == _identityUser.Id)) throw new CodeException("不能点赞自己");

            var entity = _snsdbContext.Thumbs.FirstOrDefault(x => x.AccountId == _identityUser.Id && x.ArticleId == articleId);
            if (entity != null)
            {
                _snsdbContext.Thumbs.Remove(entity);
            }
            else
            {
                entity = new Thumb
                {
                    AccountId = _identityUser.Id,
                    ArticleId = articleId
                };
                _snsdbContext.Thumbs.Add(entity);
            }
            _snsdbContext.SaveChanges();
        }
    }
}

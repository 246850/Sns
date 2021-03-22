using Calamus.AspNetCore.Users;
using Calamus.Infrastructure.Models;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System.Linq;

namespace Sns.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly SnsdbContext _snsdbContext;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        public FavoriteService(SnsdbContext snsdbContext, IIdentityUser<int, LoginAuthModel> identityUser)
        {
            _snsdbContext = snsdbContext;
            _identityUser = identityUser;
        }
        public void CreateOrRemove(int articleId)
        {
            if (_snsdbContext.Articles.Any(x => x.Id == articleId && x.AccountId == _identityUser.Id)) throw new CodeException("不能收藏自己");

            var entity = _snsdbContext.Favorites.FirstOrDefault(x => x.AccountId == _identityUser.Id && x.ArticleId == articleId);
            if (entity != null)
            {
                _snsdbContext.Favorites.Remove(entity);
            }
            else
            {
                entity = new Favorite
                {
                    AccountId = _identityUser.Id,
                    ArticleId = articleId
                };
                _snsdbContext.Favorites.Add(entity);
            }
            _snsdbContext.SaveChanges();
        }
    }
}

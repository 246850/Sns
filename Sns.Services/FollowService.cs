using Calamus.AspNetCore.Users;
using Calamus.Infrastructure.Models;
using Calamus.Result;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sns.Services
{
    public class FollowService : IFollowService
    {
        private readonly SnsdbContext _snsdbContext;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        public FollowService(SnsdbContext snsdbContext, IIdentityUser<int, LoginAuthModel> identityUser)
        {
            _snsdbContext = snsdbContext;
            _identityUser = identityUser;
        }
        public void CreateOrRemove(int followId)
        {
            if(followId == _identityUser.Id) throw new CodeException("不能关注自己");

            var entity = _snsdbContext.Follows.FirstOrDefault(x => x.AccountId == _identityUser.Id && x.FollowId == followId);
            if (entity != null)
            {
                _snsdbContext.Follows.Remove(entity);
            }
            else
            {
                entity = new Follow
                {
                    AccountId = _identityUser.Id,
                    FollowId = followId
                };
                _snsdbContext.Follows.Add(entity);
            }
            _snsdbContext.SaveChanges();
        }

        public void Remove(int followId)
        {
            var entity = _snsdbContext.Follows.FirstOrDefault(x => x.AccountId == _identityUser.Id && x.FollowId == followId);
            if (entity != null)
            {
                _snsdbContext.Follows.Remove(entity);
            }
            _snsdbContext.SaveChanges();
        }

        public IPagedList<AccountOfFollowResultDTO> GetMyFollowPagedList(int page, int pageSize)
            => BuildFollowPagedList(_identityUser.Id, page, pageSize);

        public IPagedList<AccountOfFollowResultDTO> GetMyFansPagedList(int page, int pageSize)
            => BuildFansPagedList(_identityUser.Id, page, pageSize);

        public IPagedList<AccountOfFollowResultDTO> GetPeopleFollowPagedList(int id, int page, int pageSize)
            => BuildFollowPagedList(id, page, pageSize);

        public IPagedList<AccountOfFollowResultDTO> GetPeopleFansPagedList(int id, int page, int pageSize)
            => BuildFansPagedList(id, page, pageSize);

        IPagedList<AccountOfFollowResultDTO> BuildFollowPagedList(int accountId, int page, int pageSize)
        {
            IQueryable<AccountOfFollowResultDTO> queryable = from t1 in _snsdbContext.Follows
                                                             join t2 in _snsdbContext.Accounts on t1.FollowId equals t2.Id
                                                             where t1.AccountId == accountId
                                                             orderby t1.Id descending
                                                             select new AccountOfFollowResultDTO
                                                             {
                                                                 Id = t2.Id,
                                                                 NickName = t2.NickName,
                                                                 Avatar = t2.Avatar
                                                             };
            IPagedList<AccountOfFollowResultDTO> items = queryable.ToPagedList<AccountOfFollowResultDTO>(page, pageSize);
            return items;
        }
        IPagedList<AccountOfFollowResultDTO> BuildFansPagedList(int followId, int page, int pageSize)
        {
            IQueryable<AccountOfFollowResultDTO> queryable = from t1 in _snsdbContext.Follows
                                                             join t2 in _snsdbContext.Accounts on t1.AccountId equals t2.Id
                                                             where t1.FollowId == followId
                                                             orderby t1.Id descending
                                                             select new AccountOfFollowResultDTO
                                                             {
                                                                 Id = t2.Id,
                                                                 NickName = t2.NickName,
                                                                 Avatar = t2.Avatar
                                                             };
            IPagedList<AccountOfFollowResultDTO> items = queryable.ToPagedList<AccountOfFollowResultDTO>(page, pageSize);
            return items;
        }

    }
}

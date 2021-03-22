using Calamus.AspNetCore.Users;
using Calamus.Data;
using Calamus.Infrastructure.Extensions;
using Calamus.Infrastructure.Models;
using Calamus.Infrastructure.Utils;
using Calamus.Result;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sns.Services
{
    public class AccountService : IAccountService
    {
        private readonly SnsdbContext _snsdbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        public AccountService(SnsdbContext snsdbContext,
             IWebHostEnvironment webHostEnvironment,
             IIdentityUser<int, LoginAuthModel> identityUser)
        {
            _snsdbContext = snsdbContext;
            _webHostEnvironment = webHostEnvironment;
            _identityUser = identityUser;
        }

        public bool CheckNotExists(string account)
        {
            if (string.IsNullOrWhiteSpace(account)) return false;
            return !_snsdbContext.Accounts.AsNoTracking().Any(x => x.Account1 == account.Trim());
        }

        public LoginAuthModel Login(AccountLoginRequestDTO request)
        {
            string pwd = Md5Helper.Encrypt(request.Password);
            var q1 = from t1 in _snsdbContext.Accounts
                     where t1.Account1 == request.Account && t1.Pwd == pwd
                     select new LoginAuthModel
                     {
                         Id = t1.Id,
                         Account = t1.Account1,
                         NickName = t1.NickName,
                         Avatar = t1.Avatar,
                         Intro = t1.Intro,
                         CreateTime = t1.CreateTime
                     };
            LoginAuthModel model = q1.FirstOrDefault();
            return model;
        }

        public LoginAuthModel Register(AccountRegisterRequestDTO request)
        {
            int index = RandomHelper.Default.Next(1, 101);
            Account entity = new Account
            {
                Account1 = request.Account,
                NickName = request.NickName,
                Pwd = Md5Helper.Encrypt(request.Password),
                Avatar = $"/heads/{index}_100.gif",
                Intro = string.Empty
            };
            _snsdbContext.Accounts.Add(entity);
            _snsdbContext.SaveChanges();

            return EnityToModel(entity);
        }

        public List<AccountListResultDTO> TopN(int top)
        {
            return _snsdbContext.Accounts.Select(x => new AccountListResultDTO
            {
                Id = x.Id,
                Account = x.Account1,
                NickName = x.NickName,
                Avatar = x.Avatar
            }).OrderByDescending(x => x.Id).Take(top).ToList();
        }

        public IPagedList<AccountListResultDTO> List(int page, int pageSize)
        {
            IQueryable<AccountListResultDTO> queryable = _snsdbContext.Accounts.Select(x => new AccountListResultDTO
            {
                Id = x.Id,
                Account = x.Account1,
                NickName = x.NickName,
                Avatar = x.Avatar
            }).OrderByDescending(x => x.Id);
            return queryable.ToPagedList(page, pageSize);
        }

        public AccountDetailResultDTO DetailOfMine(int id)
        {
            var model = _snsdbContext.Accounts.Select(x => new AccountDetailResultDTO
            {
                Id = x.Id,
                Account = x.Account1,
                NickName = x.NickName,
                Avatar = x.Avatar,
                Intro = x.Intro,
                CreateTime = x.CreateTime
            }).FirstOrDefault(x => x.Id == id).ThrowIfNull();

            return model;
        }
        public AccountDetailResultDTO DetailOfPeople(int id)
        {
            var model = _snsdbContext.Accounts.Select(x => new AccountDetailResultDTO
            {
                Id = x.Id,
                Account = x.Account1,
                NickName = x.NickName,
                Avatar = x.Avatar,
                Intro = x.Intro,
                CreateTime = x.CreateTime,
                IsFollowed = _snsdbContext.Follows.Any(a => a.AccountId == _identityUser.Id && a.FollowId == x.Id)
            }).FirstOrDefault(x => x.Id == id).ThrowIfNull();

            return model;
        }

        public List<string> Heads()
        {
            string folder = Path.Combine(_webHostEnvironment.WebRootPath, "heads");
            var fileNames = Directory.GetFiles(folder, "*_100.gif").Select(item => Path.GetFileName(item)).OrderBy(x => x, new AvatarComparer());
            return fileNames.ToList();
        }

        public LoginAuthModel ModifyHead(string name)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "heads", name);
            if (!File.Exists(filePath)) throw new Exception("无效的头像地址");

            Account entity = _snsdbContext.Accounts.Find(_identityUser.Id);
            entity.Avatar = $"/heads/{name}";
            _snsdbContext.SaveChanges();

            return EnityToModel(entity);
        }

        LoginAuthModel EnityToModel(Account entity)
        {
            return new LoginAuthModel
            {
                Id = entity.Id,
                Account = entity.Account1,
                NickName = entity.NickName,
                Avatar = entity.Avatar,
                Intro = entity.Intro,
                CreateTime = entity.CreateTime
            };
        }

        public LoginAuthModel UpdateIntro(AccountUpdateIntroRequestDTO request)
        {
            Account entity = _snsdbContext.Accounts.Find(_identityUser.Id);
            entity.Intro = request.Intro;
            _snsdbContext.SaveChanges();

            return EnityToModel(entity);
        }

        public void UpdatePassword(AccountUpdatePasswordRequestDTO request)
        {
            var entity = _snsdbContext.Accounts.Find(_identityUser.Id);
            if (entity.Pwd != Md5Helper.Encrypt(request.Password)) throw new CodeException("原始密码错误");
            entity.Pwd = Md5Helper.Encrypt(request.Password1);
            _snsdbContext.SaveChanges();
        }

        public IPagedList<AccountListResultDTO> GetAllPagedList(int page, int pageSize)
        {
            IQueryable<AccountListResultDTO> queryable = from t1 in _snsdbContext.Accounts
                                                         orderby t1.Id descending
                                                         select new AccountListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             NickName = t1.NickName,
                                                             Avatar = t1.Avatar
                                                         };
            return queryable.ToPagedList(page, pageSize);
        }

        public IPagedList<AccountOfArticleResultDTO> GetPopularPagedList(int page, int pageSize)
        {
            IQueryable<AccountOfArticleResultDTO> queryable = from t1 in _snsdbContext.Accounts
                                                            select new AccountOfArticleResultDTO
                                                            {
                                                                Id = t1.Id,
                                                                NickName = t1.NickName,
                                                                Avatar = t1.Avatar,
                                                                FansCount = (from a in _snsdbContext.Follows
                                                                        where a.FollowId == t1.Id
                                                                        select a.Id).Count()
                                                            };
            return queryable.OrderByDescending(x=> x.FansCount).ThenBy(x=> x.Id).ToPagedList(page, pageSize);
        }
    }
}

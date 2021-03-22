using Calamus.Ioc;
using Calamus.Result;
using Sns.Domains.Entities;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.IServices
{
    public interface IAccountService:IDependency
    {
        bool CheckNotExists(string account);
        LoginAuthModel Register(AccountRegisterRequestDTO request);
        LoginAuthModel Login(AccountLoginRequestDTO request);
        List<AccountListResultDTO> TopN(int top);
        IPagedList<AccountListResultDTO> List(int page, int pageSize);

        AccountDetailResultDTO DetailOfMine(int id);
        AccountDetailResultDTO DetailOfPeople(int id);

        List<string> Heads();
        LoginAuthModel ModifyHead(string name);

        LoginAuthModel UpdateIntro(AccountUpdateIntroRequestDTO request);
        void UpdatePassword(AccountUpdatePasswordRequestDTO request);

        IPagedList<AccountListResultDTO> GetAllPagedList(int page, int pageSize);
        IPagedList<AccountOfArticleResultDTO> GetPopularPagedList(int page, int pageSize);
    }
}

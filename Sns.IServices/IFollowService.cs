using Calamus.Ioc;
using Calamus.Result;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.IServices
{
    public interface IFollowService:IDependency
    {
        void CreateOrRemove(int followId);
        void Remove(int followId);
        IPagedList<AccountOfFollowResultDTO> GetMyFollowPagedList(int page, int pageSize);
        IPagedList<AccountOfFollowResultDTO> GetMyFansPagedList(int page, int pageSize);
        IPagedList<AccountOfFollowResultDTO> GetPeopleFollowPagedList(int id, int page, int pageSize);
        IPagedList<AccountOfFollowResultDTO> GetPeopleFansPagedList(int id, int page, int pageSize);
    }
}

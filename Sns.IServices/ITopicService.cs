using Calamus.Ioc;
using Calamus.Result;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.IServices
{
    public interface ITopicService: IDependency
    {
        TopicOfArticleResultDTO Get(string name);
        List<TopicListResultDTO> GetHotList(int pageSize);
        IPagedList<TopicListResultDTO> GetAllPagedList(int page, int pageSize);
        List<TopicListResultDTO> GetTopN(int pageSize);
    }
}

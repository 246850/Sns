using Calamus.Result;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sns.Services
{
    public class TopicService : ITopicService
    {
        private readonly SnsdbContext _snsdbContext;
        public TopicService(SnsdbContext snsdbContext)
        {
            _snsdbContext = snsdbContext;
        }

        public TopicOfArticleResultDTO Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) name = string.Empty;

            var queryable = from t1 in _snsdbContext.Topics
                            where t1.Name == name.Trim()
                            select new TopicOfArticleResultDTO
                            {
                                Id = t1.Id,
                                Name = t1.Name,
                                ArticleCount = (from a in _snsdbContext.ArticleTopics
                                                join b in _snsdbContext.Articles on a.ArticleId equals b.Id
                                                where a.TopicId == t1.Id
                                                select a.Id
                                               ).Count()
                            };
            return queryable.FirstOrDefault();
        }

        public IPagedList<TopicListResultDTO> GetAllPagedList(int page, int pageSize)
        {
            IQueryable<TopicListResultDTO> queryable = from t1 in _snsdbContext.Topics
                                                       orderby t1.Id descending
                                                       select new TopicListResultDTO
                                                       {
                                                           Id = t1.Id,
                                                           Name = t1.Name,
                                                           ArticleCount = (from a in _snsdbContext.ArticleTopics
                                                                           join b in _snsdbContext.Articles on a.ArticleId equals b.Id
                                                                           where a.TopicId == t1.Id
                                                                           select a.Id
                                                                          ).Count()
                                                       };
            return queryable.ToPagedList(page, pageSize);
        }

        public List<TopicListResultDTO> GetHotList(int pageSize)
            => GetTopN(pageSize);

        public List<TopicListResultDTO> GetTopN(int pageSize)
        {
            IQueryable<TopicListResultDTO> queryable = from t1 in _snsdbContext.Topics
                                                       let counts = (from a in _snsdbContext.ArticleTopics
                                                                     join b in _snsdbContext.Articles on a.ArticleId equals b.Id
                                                                     where a.TopicId == t1.Id
                                                                     select a.Id)
                                                       select new TopicListResultDTO
                                                       {
                                                           Id = t1.Id,
                                                           Name = t1.Name,
                                                           ArticleCount = counts.Count()
                                                       };
            return queryable.OrderByDescending(x => x.ArticleCount).ThenByDescending(x => x.Id).Take(pageSize).ToList();
        }
    }
}

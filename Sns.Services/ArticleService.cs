using Calamus.AspNetCore.Users;
using Calamus.Caching;
using Calamus.Data;
using Calamus.Infrastructure.Extensions;
using Calamus.Infrastructure.Utils;
using Calamus.Ioc;
using Calamus.Result;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Sns.Domains.Entities;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sns.Services
{
    public class ArticleService : IArticleService
    {
        private readonly SnsdbContext _snsdbContext;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ArticleService(SnsdbContext snsdbContext, IIdentityUser<int, LoginAuthModel> identityUser,
            IDistributedCache distributedCache,
            IHttpContextAccessor httpContextAccessor)
        {
            _snsdbContext = snsdbContext;
            _identityUser = identityUser;
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
        }

        public int CreateOrUpdate(ArticleCreateOrUpdateRequestDTO request)
        {
            if (!_snsdbContext.Articles.Any(x => x.Id == request.Id))
            {
                Article article = new Article
                {
                    Title = request.Title,
                    AccountId = _identityUser.Id,
                    ContentType = ContentTypeEnum.Article.ToValue()
                };
                ArticleContent content = new ArticleContent
                {
                    Body = request.Body
                };
                Topic topic = _snsdbContext.Topics.AsNoTracking().FirstOrDefault(x => x.Name == request.TopicName);

                _snsdbContext.ExecuteTransaction(() =>
                {
                    // 文章
                    _snsdbContext.Articles.Add(article);
                    _snsdbContext.SaveChanges();
                    // 内容
                    content.ArticleId = article.Id;
                    _snsdbContext.ArticleContents.Add(content);
                    _snsdbContext.SaveChanges();
                    // 话题
                    if (topic == null)
                    {
                        topic = new Topic
                        {
                            Name = request.TopicName
                        };
                        _snsdbContext.Topics.Add(topic);
                        _snsdbContext.SaveChanges();
                    }
                    // 文章|话题 关系
                    ArticleTopic articleTopic = new ArticleTopic
                    {
                        ArticleId = article.Id,
                        TopicId = topic.Id
                    };
                    _snsdbContext.ArticleTopics.Add(articleTopic);
                    _snsdbContext.SaveChanges();
                });
                return article.Id;
            }
            else
            {
                // 文章
                Article article = _snsdbContext.Articles.Find(request.Id);
                article.Title = request.Title;
                article.LastUpdateTime = DateTime.Now;
                // 内容
                ArticleContent articleContent = _snsdbContext.ArticleContents.FirstOrDefault(x => x.ArticleId == article.Id);
                if (articleContent != null)
                {
                    articleContent.Body = request.Body;
                }
                // 话题
                List<ArticleTopic> articleTopics = _snsdbContext.ArticleTopics.Where(x => x.ArticleId == article.Id).ToList();
                _snsdbContext.ArticleTopics.RemoveRange(articleTopics); // 先清空老的关系
                Topic topic = _snsdbContext.Topics.AsNoTracking().FirstOrDefault(x => x.Name == request.TopicName);
                _snsdbContext.ExecuteTransaction(() =>
                {
                    if (topic == null)
                    {
                        topic = new Topic
                        {
                            Name = request.TopicName
                        };
                        _snsdbContext.Topics.Add(topic);
                        _snsdbContext.SaveChanges();
                    }

                    // 文章|话题 关系
                    ArticleTopic articleTopic = new ArticleTopic
                    {
                        ArticleId = article.Id,
                        TopicId = topic.Id
                    };
                    _snsdbContext.ArticleTopics.Add(articleTopic);
                    _snsdbContext.SaveChanges();
                });
                return article.Id;
            }
        }

        public ArticleCreateOrUpdateRequestDTO Edit(int id)
        {
            IQueryable<ArticleCreateOrUpdateRequestDTO> queryable = from t1 in _snsdbContext.Articles
                                                                    join t2 in _snsdbContext.ArticleContents on t1.Id equals t2.ArticleId
                                                                    join t3 in _snsdbContext.ArticleTopics on t1.Id equals t3.ArticleId into temp1
                                                                    from t3 in temp1.DefaultIfEmpty()
                                                                    join t4 in _snsdbContext.Topics on t3.TopicId equals t4.Id into temp2
                                                                    from t4 in temp2.DefaultIfEmpty()
                                                                    select new ArticleCreateOrUpdateRequestDTO
                                                                    {
                                                                        Id = t1.Id,
                                                                        Title = t1.Title,
                                                                        Body = t2.Body,
                                                                        TopicName = t4 != null ? t4.Name : string.Empty
                                                                    };
            return queryable.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
            _snsdbContext.RemoveRange<Article>(x => x.Id == id);
            _snsdbContext.SaveChanges();
        }

        public IPagedList<ArticleListResultDTO> List(int page, int pageSize, string topicName = "")
        {
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                         join t2 in _snsdbContext.ArticleContents on t1.Id equals t2.ArticleId
                                                         join t3 in _snsdbContext.Accounts on t1.AccountId equals t3.Id
                                                         join t4 in _snsdbContext.ArticleTopics on t1.Id equals t4.ArticleId into temp1
                                                         from t4 in temp1.DefaultIfEmpty()
                                                         join t5 in _snsdbContext.Topics on t4.TopicId equals t5.Id into temp2
                                                         from t5 in temp2.DefaultIfEmpty()
                                                         where string.IsNullOrWhiteSpace(topicName) || t5.Name == topicName.Trim()
                                                         orderby t1.Id descending
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             Title = t1.Title,
                                                             SubTitle = t2.Body,
                                                             CreateTime = t1.CreateTime,
                                                             AccountInfo = new AccountOfArticleResultDTO
                                                             {
                                                                 Id = t3.Id,
                                                                 NickName = t3.NickName,
                                                                 Account = t3.Account1
                                                             },
                                                             ViewCount = t1.ViewCount,
                                                             ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t1.Id).Count(),
                                                             CommentCount = (from a in _snsdbContext.Comments
                                                                             join b in _snsdbContext.Accounts on a.AccountId equals b.Id
                                                                             where a.ArticleId == t1.Id
                                                                             select a.Id).Count()
                                                         };
            IPagedList<ArticleListResultDTO> items = queryable.ToPagedList<ArticleListResultDTO>(page, pageSize);
            foreach (ArticleListResultDTO item in items)
            {
                item.SubTitle = item.SubTitle.FilterHtml().TrimStart().CutWithSuffix(60);
            }
            return items;
        }

        public async Task<ArticleDetailResultDTO> Detail(int id, int page, int pageSize)
        {
            IQueryable<ArticleDetailResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                           join t2 in _snsdbContext.ArticleContents on t1.Id equals t2.ArticleId
                                                           join t3 in _snsdbContext.Accounts on t1.AccountId equals t3.Id
                                                           join t4 in _snsdbContext.ArticleTopics on t1.Id equals t4.ArticleId into temp1
                                                           from t4 in temp1.DefaultIfEmpty()
                                                           join t5 in _snsdbContext.Topics on t4.TopicId equals t5.Id into temp2
                                                           from t5 in temp2.DefaultIfEmpty()
                                                           select new ArticleDetailResultDTO
                                                           {
                                                               Id = t1.Id,
                                                               Title = t1.Title,
                                                               Body = t2.Body,
                                                               ViewCount = t1.ViewCount,
                                                               CreateTime = t1.CreateTime,
                                                               LastUpdateTime = t1.LastUpdateTime,
                                                               AccountInfo = new AccountOfArticleResultDTO
                                                               {
                                                                   Id = t3.Id,
                                                                   NickName = t3.NickName,
                                                                   Account = t3.Account1,
                                                                   Avatar = t3.Avatar,
                                                                   Intro = t3.Intro,

                                                                   ArticleCount = _snsdbContext.Articles.Where(x => x.AccountId == t3.Id).Count(),

                                                                   FansCount = _snsdbContext.Follows.Where(x => x.FollowId == t3.Id).Count(),  // 发帖子关注者数量
                                                                   IsFollow = _snsdbContext.Follows.Any(x => x.AccountId == _identityUser.Id && x.FollowId == t3.Id) // 当前登录人是否关注 发帖人
                                                               },
                                                               TopicInfo = t5 != null ? new TopicOfArticleResultDTO
                                                               {
                                                                   Id = t5.Id,
                                                                   Name = t5.Name
                                                               } : null,
                                                               IsThumbup = _snsdbContext.Thumbs.Any(x => x.AccountId == _identityUser.Id && x.ArticleId == t1.Id),
                                                               ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t1.Id).Count(),
                                                               IsFavorite = _snsdbContext.Favorites.Any(x => x.AccountId == _identityUser.Id && x.ArticleId == t1.Id)
                                                           };
            ArticleDetailResultDTO item = await queryable.FirstOrDefaultAsync(x => x.Id == id);

            IQueryable<CommentOfArticleDetailResultDTO> q2 = from t1 in _snsdbContext.Comments
                                                             join t2 in _snsdbContext.Accounts on t1.AccountId equals t2.Id
                                                             join t3 in _snsdbContext.Accounts on t1.QuoteId equals t3.Id into temp
                                                             from t3 in temp.DefaultIfEmpty()
                                                             where t1.ArticleId == item.Id
                                                             orderby t1.Id ascending
                                                             select new CommentOfArticleDetailResultDTO
                                                             {
                                                                 Id = t1.Id,
                                                                 Body = t1.Body,
                                                                 CreateTime = t1.CreateTime,
                                                                 AccountInfo = new AccountOfArticleResultDTO
                                                                 {
                                                                     Id = t2.Id,
                                                                     Account = t2.Account1,
                                                                     NickName = t2.NickName,
                                                                     Avatar = t2.Avatar
                                                                 },
                                                                 QuoteInfo = t3 != null ? new AccountOfArticleResultDTO
                                                                 {
                                                                     Id = t3.Id,
                                                                     Account = t3.Account1,
                                                                     NickName = t3.NickName
                                                                 } : null,
                                                                 IsThumbup = _snsdbContext.CommentThumbs.Any(x => x.AccountId == _identityUser.Id && x.CommentId == t1.Id),
                                                                 ThumbupCount = _snsdbContext.CommentThumbs.Where(x => x.CommentId == t1.Id).Count()
                                                             };
            IPagedList<CommentOfArticleDetailResultDTO> comments = q2.ToPagedList<CommentOfArticleDetailResultDTO>(page, pageSize);
            item.Comments = comments;

            _distributedCache.GetOrAdd<object>($"{CacheKeyConstant.ArticleViewReadCacheKey}_{_httpContextAccessor.HttpContext.Connection.RemoteIpAddress}_{id}", () =>
                _snsdbContext.Database.ExecuteSqlRaw($@"UPDATE article 
                                                        SET ViewCount = ViewCount + 1 
                                                        WHERE
	                                                        Id = {id}"), 3600); // 同一IP和文章ID，2小时只记1条阅读
            return item;
        }

        public IPagedList<ArticleListResultDTO> GetMyPagedList(int page, int pageSize)
        {
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                         where t1.AccountId == _identityUser.Id
                                                         orderby t1.Id descending
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             Title = t1.Title,
                                                             CreateTime = t1.CreateTime,
                                                             ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t1.Id).Count(),
                                                             CommentCount = _snsdbContext.Comments.Where(x => x.ArticleId == t1.Id).Count()
                                                         };
            IPagedList<ArticleListResultDTO> items = queryable.ToPagedList<ArticleListResultDTO>(page, pageSize);
            return items;
        }

        public IPagedList<ArticleListResultDTO> GetMyCommentPagedList(int page, int pageSize)
        {
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                         where t1.AccountId != _identityUser.Id && _snsdbContext.Comments.Any(x => x.AccountId == _identityUser.Id && x.ArticleId == t1.Id)
                                                         orderby t1.Id descending
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             Title = t1.Title,
                                                             CreateTime = t1.CreateTime,
                                                             ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t1.Id).Count(),
                                                             CommentCount = _snsdbContext.Comments.Where(x => x.ArticleId == t1.Id).Count()
                                                         };
            IPagedList<ArticleListResultDTO> items = queryable.ToPagedList<ArticleListResultDTO>(page, pageSize);
            return items;
        }

        public IPagedList<ArticleListResultDTO> GetMyThumbupPagedList(int page, int pageSize)
        {
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Thumbs
                                                         join t2 in _snsdbContext.Articles on t1.ArticleId equals t2.Id
                                                         where t1.AccountId == _identityUser.Id && t2.AccountId != _identityUser.Id
                                                         orderby t1.Id descending
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t2.Id,
                                                             Title = t2.Title,
                                                             CreateTime = t2.CreateTime,
                                                             ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t2.Id).Count(),
                                                             CommentCount = _snsdbContext.Comments.Where(x => x.ArticleId == t2.Id).Count()
                                                         };
            IPagedList<ArticleListResultDTO> items = queryable.ToPagedList<ArticleListResultDTO>(page, pageSize);
            return items;
        }

        public IPagedList<ArticleListResultDTO> GetMyFavoritePagedList(int page, int pageSize)
        {
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Favorites
                                                         join t2 in _snsdbContext.Articles on t1.ArticleId equals t2.Id
                                                         where t1.AccountId == _identityUser.Id && t2.AccountId != _identityUser.Id
                                                         orderby t1.Id descending
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t2.Id,
                                                             Title = t2.Title,
                                                             CreateTime = t2.CreateTime,
                                                             ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t2.Id).Count(),
                                                             CommentCount = _snsdbContext.Comments.Where(x => x.ArticleId == t2.Id).Count()
                                                         };
            IPagedList<ArticleListResultDTO> items = queryable.ToPagedList<ArticleListResultDTO>(page, pageSize);
            return items;
        }

        public IPagedList<ArticleListResultDTO> GetPeoplePagedList(int id, int page, int pageSize)
        {
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                         where t1.AccountId == id
                                                         orderby t1.Id descending
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             Title = t1.Title,
                                                             CreateTime = t1.CreateTime,
                                                             ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t1.Id).Count(),
                                                             CommentCount = _snsdbContext.Comments.Where(x => x.ArticleId == t1.Id).Count()
                                                         };
            IPagedList<ArticleListResultDTO> items = queryable.ToPagedList<ArticleListResultDTO>(page, pageSize);
            return items;
        }

        public List<ArticleListResultDTO> Get10DayOfThumbupList(int pageSize)
        {
            DateTime begin = DateTime.Now.AddDays(-10).Date;
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                         join t2 in _snsdbContext.Accounts on t1.AccountId equals t2.Id
                                                         where t1.CreateTime >= begin
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             Title = t1.Title,
                                                             ThumbupCount = _snsdbContext.Thumbs.Where(x => x.ArticleId == t1.Id).Count()
                                                         };
            return queryable.OrderByDescending(x => x.ThumbupCount).ThenBy(x => x.Id).Take(pageSize).ToList();
        }

        public List<ArticleListResultDTO> Get2DayOfCommentList(int pageSize)
        {
            DateTime begin = DateTime.Now.AddDays(-2).Date;
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                         join t2 in _snsdbContext.Accounts on t1.AccountId equals t2.Id
                                                         where t1.CreateTime >= begin
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             Title = t1.Title,
                                                             CommentCount = _snsdbContext.Comments.Where(x => x.ArticleId == t1.Id).Count()
                                                         };
            return queryable.OrderByDescending(x => x.CommentCount).ThenBy(x => x.Id).Take(pageSize).ToList();
        }

        public List<ArticleListResultDTO> Get1MonthOfViewList(int pageSize)
        {
            DateTime begin = DateTime.Now.AddMonths(-1).Date;
            IQueryable<ArticleListResultDTO> queryable = from t1 in _snsdbContext.Articles
                                                         join t2 in _snsdbContext.Accounts on t1.AccountId equals t2.Id
                                                         where t1.CreateTime >= begin
                                                         orderby t1.ViewCount descending, t1.Id ascending
                                                         select new ArticleListResultDTO
                                                         {
                                                             Id = t1.Id,
                                                             Title = t1.Title
                                                         };
            return queryable.Take(pageSize).ToList();
        }

    }
}

using Calamus.Ioc;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Calamus.Result;

namespace Sns.IServices
{
    public interface IArticleService:IDependency
    {
        int CreateOrUpdate(ArticleCreateOrUpdateRequestDTO request);
        ArticleCreateOrUpdateRequestDTO Edit(int id);
        IPagedList<ArticleListResultDTO> List(int page, int pageSize, string topicName = "");

        Task<ArticleDetailResultDTO> Detail(int id, int page, int pageSize);
        void Delete(int id);

        IPagedList<ArticleListResultDTO> GetMyPagedList(int page, int pageSize);

        IPagedList<ArticleListResultDTO> GetMyCommentPagedList(int page, int pageSize);
        IPagedList<ArticleListResultDTO> GetMyThumbupPagedList(int page, int pageSize);
        IPagedList<ArticleListResultDTO> GetMyFavoritePagedList(int page, int pageSize);

        IPagedList<ArticleListResultDTO> GetPeoplePagedList(int id, int page, int pageSize);

        List<ArticleListResultDTO> Get10DayOfThumbupList(int pageSize);
        List<ArticleListResultDTO> Get2DayOfCommentList(int pageSize);

        List<ArticleListResultDTO> Get1MonthOfViewList(int pageSize);

    }
}

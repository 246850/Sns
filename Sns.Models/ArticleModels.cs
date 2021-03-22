using Calamus.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.Models
{
    #region requests
    public class ArticleCreateOrUpdateRequestDTO
    {
        public int Id { get; set; }

        private string _title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_title)) return string.Empty;
                return _title.Trim();
            }
            set
            {
                _title = value;
            }
        }
        /// <summary>
        /// 话题名称
        /// </summary>
        public string TopicName { get; set; }
        /// <summary>
        /// 内容体，3000个字符长度
        /// </summary>
        public string Body { get; set; }
    }

    public class ImageUploadRequestDTO
    {
        public string Base64 { get; set; }
    }
    #endregion

    #region responses
    public class ArticleListResultDTO
    {
        public int Id { get; set; }
        public AccountOfArticleResultDTO AccountInfo { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 子内容，50个字符串长度
        /// </summary>
        public string SubTitle { get; set; }
        public DateTime CreateTime { get; set; }
        public int ViewCount { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int ThumbupCount { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }
    }

    public class ArticleDetailResultDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容体，3000个字符长度
        /// </summary>
        public string Body { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public AccountOfArticleResultDTO AccountInfo { get; set; }
        public TopicOfArticleResultDTO TopicInfo { get; set; }
        /// <summary>
        /// 当前用户是否点赞
        /// </summary>
        public bool IsThumbup { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int ThumbupCount { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsFavorite { get; set; }
        public IPagedList<CommentOfArticleDetailResultDTO> Comments { get; set; }
    }

    public class ImageUploadResultDTO
    {
        public string Path { get; set; }
        public string Host { get; set; }
    }

    #endregion
}

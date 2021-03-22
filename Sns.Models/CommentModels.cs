using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.Models
{
    #region Requests
    public class CommentCreateRequestDTO
    {
        /// <summary>
        /// 引用ID
        /// </summary>
        public int QuoteId { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        public int ArticleId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Body { get; set; }
    }
    #endregion

    #region Responses
    public class CommentOfArticleDetailResultDTO
    {
        public int Id { get; set; }
        public AccountOfArticleResultDTO AccountInfo { get; set; }
        public AccountOfArticleResultDTO QuoteInfo { get; set; }
        public string Body { get; set; }
        public bool IsThumbup { get; set; }
        public int ThumbupCount { get; set; }
        public DateTime CreateTime { get; set; }
    }
    #endregion
}

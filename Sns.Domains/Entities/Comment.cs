// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Sns.Domains.Entities
{
    /// <summary>
    /// 评论表
    /// </summary>
    public partial class Comment
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int AccountId { get; set; }
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
        public DateTime CreateTime { get; set; }
    }
}
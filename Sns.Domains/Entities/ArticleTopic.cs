﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Sns.Domains.Entities
{
    public partial class ArticleTopic
    {
        public int Id { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        public int ArticleId { get; set; }
        /// <summary>
        /// 话题ID
        /// </summary>
        public int TopicId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
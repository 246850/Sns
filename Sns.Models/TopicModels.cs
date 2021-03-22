using System;
using System.Collections.Generic;
using System.Text;

namespace Sns.Models
{
    #region Response
    public class TopicOfArticleResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArticleCount { get; set; }
    }

    public class TopicListResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArticleCount { get; set; }
    }
    #endregion
}

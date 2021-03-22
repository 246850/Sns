using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Models
{
    public abstract class RequestBase
    {
        public RequestBase()
        {
            Pager = new PageRequest();
            Sorts = new List<Sorting>();
        }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageRequest Pager { get; set; }
        /// <summary>
        /// 排序信息
        /// </summary>
        public List<Sorting> Sorts { get; set; }
    }
}

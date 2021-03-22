using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Models
{
    public class PageRequest
    {
        public PageRequest()
        {
            Page = 1;
            PageSize = 10;
        }
        private int _page;
        /// <summary>
        /// 当前页
        /// </summary>
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                if (value <= 0) value = 1;
                _page = value;
            }
        }
        private int _pageSize;
        /// <summary>
        /// 页记录大小
        /// </summary>
        public int PageSize 
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0) value = 10;
                _pageSize = value;
            }
        }
    }
}

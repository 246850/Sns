using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Calamus.Result
{
    /// <summary>
    /// 页码对象
    /// </summary>
    public class PageModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int Page { get; }
        /// <summary>
        /// 页容量大小
        /// </summary>
        public int PageSize { get; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public long Total { get; }
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPage
        {
            get
            {
                if (PageSize == 0) return 0;
                return (Total + PageSize - 1) / PageSize;
            }
        }
        /// <summary>
        /// 第几行
        /// </summary>
        [JsonIgnore]
        public int RowNumber
        {
            get
            {
                return (Page - 1) * PageSize + 1;
            }
        }
        /// <summary>
        /// 页容量大小数组
        /// </summary>
        [JsonIgnore]
        public int[] Records { get; private set; }
        /// <summary>
        /// 是否存在上一页
        /// </summary>
        [JsonIgnore]
        public bool HasPreviousPage { get { return Page > 1 && TotalPage > 1; } }
        /// <summary>
        /// 是否存在下一页
        /// </summary>
        [JsonIgnore]
        public bool HasNextPage { get { return Page > 0 && Page <= TotalPage - 1; } }

        public string Url { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;

        internal PageModel() : this(1, 15, 0) { }
        internal PageModel(int page, int pageSize, long total)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            Records = new[] { 15, 20, 30, 50, 100 };
        }
    }
}

using System.Collections.Generic;

namespace Calamus.Result
{
    internal class PagedList<T>:List<T>, IPagedList<T> where T : class
    {
        public PageModel Pager { get; }
        public PagedList() : this(1, 15, 0) { }

        public PagedList(int page, int pageSize, long total)
        {
            Pager = new PageModel(page, pageSize, total);
        }
    }
}

using System.Collections.Generic;

namespace Calamus.Result
{
    /// <summary>
    /// 分页列表接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedList<T>:IList<T> where T:class
    {
        /// <summary>
        /// 页码信息
        /// </summary>
        PageModel Pager { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Result
{
    /// <summary>
    /// Code | Msg | Data 结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CodeResult<T> : CodeResult
    {
        public CodeResult():base()
        {
            
        }

        public CodeResult(int code, string msg):base(code, msg)
        {
                
        }
        /// <summary>
        /// 数据载体
        /// </summary>
        public virtual T Data { get; set; }
    }
}

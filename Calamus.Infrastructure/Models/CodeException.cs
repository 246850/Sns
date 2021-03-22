using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Models
{
    public class CodeException: Exception
    {
        public CodeException(string message):this(-1, message)
        {

        }
        public CodeException(int code, string message) : base(message)
        {
            Code = code;
        }
        public CodeException(int code, string message, Exception innnerException) : base(message, innnerException)
        {
            Code = code;
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
    }
}

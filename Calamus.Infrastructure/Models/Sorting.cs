using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Models
{
    /// <summary>
    /// 排序对象
    /// </summary>
    public class Sorting
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Field { get; set; }

        private string _direction;
        /// <summary>
        /// 排序方向 ASC|DESC
        /// </summary>
        public string Direction
        {
            get { return _direction; }
            set
            {
                if(string.Equals(value, "ASC", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "DESC", StringComparison.OrdinalIgnoreCase))
                {
                    _direction = value;
                }
                else
                {
                    _direction = "ASC";
                }
            }
        }

        public override string ToString()
        {
            return $"{Field} {Direction}";
        }
    }
}

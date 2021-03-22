using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Data
{
    /// <summary>
    ///  主键为：Id领域实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityBase<TKey>: EntityBase
    {
        public TKey Id { get; set; }
    }
}

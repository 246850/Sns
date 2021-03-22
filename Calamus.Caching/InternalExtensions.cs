using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Caching
{
    internal static class InternalExtensions
    {
        public static TimeSpan? ToTimeSpan(this long? source)
        {
            if (source.HasValue) return TimeSpan.FromSeconds(source.Value);
            return null;
        }
    }
}

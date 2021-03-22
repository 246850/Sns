using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calamus.Ioc
{
    /// <summary>
    /// 简易静态 单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get { return _instance; }
            set
            {
                if (_instance == null)
                {
                    _instance = value;
                }
            }
        }
    }
}

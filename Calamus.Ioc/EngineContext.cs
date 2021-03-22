using System.Runtime.CompilerServices;

namespace Calamus.Ioc
{
    /// <summary>
    /// Ioc 引擎上下文
    /// </summary>
    public static class EngineContext
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        static void Build()
        {
            if (Singleton<IEngine>.Instance == null)
            {
                Singleton<IEngine>.Instance = new DefaultEngine();
            }
        }

        /// <summary>
        /// 当前应用程序引擎实例 - 基本配置选项默认名：Apps
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Build();
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}

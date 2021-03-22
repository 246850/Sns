using System.Text.Json;

namespace Calamus.Infrastructure.TextJson
{
    public static class JsonExtensions
    {
        /// <summary>
        /// object 序列化 json
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Serialize(this object source)
        {
#if DEBUG
            return JsonSerializer.Serialize(source, new JsonSerializerOptions
            {
                WriteIndented = true
            });
#else
            return JsonSerializer.Serialize(source);
#endif
        }

        /// <summary>
        /// json 反序列化 object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string json)
            where T : class, new()
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// 简易对象转换映射，采用序列化/反序列化。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T MapTo<T>(this object source)
            where T:class, new()
        {
            string json = Serialize(source);
            return Deserialize<T>(json);
        }
    }
}

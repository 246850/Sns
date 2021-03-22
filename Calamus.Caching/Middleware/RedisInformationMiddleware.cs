using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calamus.Caching.Middleware
{
    /// <summary>
    /// Redis 服务器信息 中间件
    /// </summary>
    public class RedisInformationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _connection;
        private readonly RedisInformationOptions _options;
        public RedisInformationMiddleware(RequestDelegate next, 
            IConnectionMultiplexer connection,
            IOptions<RedisInformationOptions> options)
        {
            _next = next;
            _connection = connection;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var info = new
            {
                _connection.ClientName,
                _connection.OperationCount,
                Status = _connection.GetStatus(),
                Counters = _connection.GetCounters()
            };

            context.Response.ContentType = "application/json";
            string json = JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true});
            await context.Response.WriteAsync(json);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Data
{
    /// <summary>
    /// EF Core DbContext 开启 无跟踪 查询
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DbContextNoTrackingFilterAttribute : TypeFilterAttribute
    {
        public DbContextNoTrackingFilterAttribute() : base(typeof(DbContextNoTrackingFilter))
        {

        }

        public class DbContextNoTrackingFilter : ActionFilterAttribute
        {
            private readonly DbContext _db;
            public DbContextNoTrackingFilter(DbContext db)
            {
                _db = db;
            }
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // 无跟踪
            }

            public override void OnActionExecuted(ActionExecutedContext context)
            {
                base.OnActionExecuted(context);
            }
        }
    }
}

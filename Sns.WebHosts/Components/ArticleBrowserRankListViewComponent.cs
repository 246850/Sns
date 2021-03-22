using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Components
{
    public class ArticleBrowserRankListViewComponent : ViewComponent
    {
        private readonly IArticleService _service;
        public ArticleBrowserRankListViewComponent(IArticleService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke(int pageSize = 15)
        {
            var models = _service.Get1MonthOfViewList(pageSize);
            return View(models);
        }
    }
}

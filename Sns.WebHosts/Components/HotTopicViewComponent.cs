using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Components
{
    public class HotTopicViewComponent:ViewComponent
    {
        private readonly ITopicService _service;
        public HotTopicViewComponent(ITopicService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke(int pageSize = 15)
        {
            var models = _service.GetTopN(pageSize);
            return View(models);
        }
    }
}

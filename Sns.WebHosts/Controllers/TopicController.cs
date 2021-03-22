using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Controllers
{
    public class TopicController : Controller
    {
        private readonly ITopicService _service;
        public TopicController(ITopicService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Hot(int pageSize = 100)
        {
            var models = _service.GetHotList(pageSize);
            return View(models);
        }

        [HttpGet]
        public IActionResult All(int page = 1, int pageSize = 100)
        {
            var models = _service.GetAllPagedList(page, pageSize);
            return View(models);
        }
    }
}

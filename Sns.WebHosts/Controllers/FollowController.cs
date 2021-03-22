using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Controllers
{
    [Authorize]
    public class FollowController : Controller
    {
        private readonly IFollowService _service;
        public FollowController(IFollowService service)
        {
            _service = service;
        }
        [HttpPost]
        public void CreateOrRemove(int followId)
        {
            _service.CreateOrRemove(followId);
        }

        [HttpPost]
        public void Remove(int followId)
        {
            _service.Remove(followId);
        }
    }
}

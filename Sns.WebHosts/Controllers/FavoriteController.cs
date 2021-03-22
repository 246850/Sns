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
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _service;
        public FavoriteController(IFavoriteService service)
        {
            _service = service;
        }
        [HttpPost]
        public void CreateOrRemove(int articleId)
        {
            _service.CreateOrRemove(articleId);
        }
    }
}

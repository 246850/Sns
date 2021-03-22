using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IFollowService _followService;
        private readonly IArticleService _articleService;
        private readonly IAccountService _accountService;
        public PeopleController(IFollowService followService,
            IArticleService articleService,
            IAccountService accountService)
        {
            _followService = followService;
            _articleService = articleService;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Article(int id, int page = 1, int pageSize = 20)
        {
            var models = _articleService.GetPeoplePagedList(id, page, pageSize);
            var people = _accountService.DetailOfPeople(id);
            ViewBag.People = people;
            ViewBag.Id = id;
            ViewData["Title"] = people.NickName;
            return View(models);
        }

        [HttpGet]
        public IActionResult Follow(int id, int page = 1, int pageSize = 50)
        {
            var models = _followService.GetPeopleFollowPagedList(id, page, pageSize);
            var people = _accountService.DetailOfPeople(id);
            ViewBag.People = people;
            ViewBag.Id = id;
            ViewData["Title"] = people.NickName;
            return View(models);
        }

        [HttpGet]
        public IActionResult Fans(int id, int page = 1, int pageSize = 50)
        {
            var models = _followService.GetPeopleFansPagedList(id, page, pageSize);
            var people = _accountService.DetailOfPeople(id);
            ViewBag.People = people;
            ViewBag.Id = id;
            ViewData["Title"] = people.NickName;
            return View(models);
        }

        [HttpGet]
        public IActionResult All(int page = 1, int pageSize = 50)
        {
            var models = _accountService.GetAllPagedList(page, pageSize);
            return View(models);
        }

        public IActionResult Popular(int page = 1, int pageSize = 50)
        {
            var models = _accountService.GetPopularPagedList(page, pageSize);
            return View(models);
        }
    }
}

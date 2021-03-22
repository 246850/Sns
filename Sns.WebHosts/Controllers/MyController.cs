using Calamus.AspNetCore.FluentValidator;
using Calamus.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sns.WebHosts.Controllers
{
    [Authorize]
    public class MyController : Controller
    {
        private readonly IFollowService _followService;
        private readonly IAccountService _service;
        private readonly IArticleService _articleService;
        public MyController(IFollowService followService,
            IAccountService service,
            IArticleService articleService)
        {
            _followService = followService;
            _service = service;
            _articleService = articleService;
        }
        [HttpGet]
        public IActionResult Article(int page = 1, int pageSize = 20)
        {
            var models = _articleService.GetMyPagedList(page, pageSize);
            return View(models);
        }

        [HttpGet]
        public IActionResult Follow(int page =1, int pageSize = 50)
        {
            var models = _followService.GetMyFollowPagedList(page, pageSize);
            return View(models);
        }

        [HttpGet]
        public IActionResult Fans(int page = 1, int pageSize = 50)
        {
            var models = _followService.GetMyFansPagedList(page, pageSize);
            return View(models);
        }

        [HttpGet]
        public IActionResult Comment(int page = 1, int pageSize = 20)
        {
            var models = _articleService.GetMyCommentPagedList(page, pageSize);
            return View(models);
        }
        [HttpGet]
        public IActionResult Thumbup(int page = 1, int pageSize = 20)
        {
            var models = _articleService.GetMyThumbupPagedList(page, pageSize);
            return View(models);
        }

        [HttpGet]
        public IActionResult Favorite(int page = 1, int pageSize = 20)
        {
            var models = _articleService.GetMyFavoritePagedList(page, pageSize);
            return View(models);
        }

        [HttpGet]
        public IActionResult ModifyHead()
        {
            var models = _service.Heads();
            return View(models);
        }

        [HttpPost]
        public void ModifyHead(string name)
        {
            var account = _service.ModifyHead(name);
            SignIn(account);
        }

        [HttpGet]
        public　IActionResult Info()
        {
            return View();
        }

        [HttpPost, ModelValidatorFilter]
        public void UpdateIntro(AccountUpdateIntroRequestDTO request)
        {
            var account = _service.UpdateIntro(request); 
            SignIn(account);
        }

        [HttpPost, ModelValidatorFilter]
        public void UpdatePassword(AccountUpdatePasswordRequestDTO request)
        {
            _service.UpdatePassword(request);
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        void SignIn(LoginAuthModel account)
        {
            // 写入cookie
            DateTime now = DateTime.Now;
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Nbf, now.ToTimestamp().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
                 new Claim(JwtRegisteredClaimNames.Exp, now.AddDays(30).ToTimestamp().ToString()),
                 new Claim("id", account.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Sub, System.Text.Json.JsonSerializer.Serialize(account))
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.AddDays(30) // 有效期30天
            }).Wait();
        }
    }
}

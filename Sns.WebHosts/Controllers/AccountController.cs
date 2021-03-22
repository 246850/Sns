using Calamus.AspNetCore.FluentValidator;
using Calamus.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sns.WebHosts.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginRequestDTO request, string returnUrl = "")
        {
            var account = _service.Login(request);
            if(account == null)
            {
                ModelState.AddModelError("loginError", "账号/密码不正确");
                return View(request);
            }
            await SignIn(account);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("list", "article");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("list", "article");
        }

        [HttpGet]
        public IActionResult Register()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpGet]
        public IActionResult CheckNotExists(string account)
        {
            bool flag = _service.CheckNotExists(account);
            return Content(flag.ToString().ToLower());
        }

        [HttpPost, ModelValidatorFilter]
        public async Task Register(AccountRegisterRequestDTO request)
        {
            var account = _service.Register(request);
            await SignIn(account);
        }

        [HttpGet]
        public IActionResult CheckName(string nickName)
        {
            var length = Encoding.UTF8.GetByteCount(nickName);
            bool flag = Regex.IsMatch(nickName, @"^\S{3,24}") && length >= 1 && length <= 24;
            return Content(flag.ToString().ToLower());
        }

        [HttpGet]
        public IActionResult Heads()
        {
            return View();
        }

        async Task SignIn(LoginAuthModel account)
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
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.AddDays(30) // 有效期30天
            });
        }
    }
}

using Calamus.AspNetCore.FluentValidator;
using Calamus.AspNetCore.Users;
using Calamus.Infrastructure.Extensions;
using Calamus.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sns.WebHosts.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleService _service;
        private readonly IThumbService _thumbService;
        private readonly IFavoriteService _favoriteService;
        private readonly IAccountService _accountService;
        private readonly IFollowService _followService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ITopicService _topicService;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        public ArticleController(ILogger<ArticleController> logger, 
            IArticleService service,
            IThumbService thumbService,
            IFavoriteService favoriteService,
            IAccountService accountService,
            IFollowService followService,
            IWebHostEnvironment hostEnvironment,
            ITopicService topicService,
            IIdentityUser<int, LoginAuthModel> identityUser)
        {
            _logger = logger;
            _service = service;
            _thumbService = thumbService;
            _favoriteService = favoriteService;
            _accountService = accountService;
            _followService = followService;
            _hostEnvironment = hostEnvironment;
            _topicService = topicService;
            _identityUser = identityUser;
        }
        public IActionResult List(string topicName = "", int page = 1, int pageSize = 15)
        {
            var topic = _topicService.Get(topicName);
            var models = _service.List(page, pageSize, topicName);
            models.Pager.QueryString = $"&topicName={topicName}";
            var accounts = _accountService.TopN(7);
            var thumbups = _service.Get10DayOfThumbupList(10);
            var comments = _service.Get2DayOfCommentList(10);
            ViewBag.TopicModel = topic;
            ViewBag.TopNAccountList = accounts;
            ViewBag.TopNThumbupList = thumbups;
            ViewBag.TopNCommentList = comments;
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id, int page = 1, int pageSize = 15)
        {
            var model = await _service.Detail(id, page, pageSize);
            return View(model);
        }

        [HttpGet, Authorize]
        public IActionResult Publish(int id)
        {
            ArticleCreateOrUpdateRequestDTO model = _service.Edit(id) ?? new ArticleCreateOrUpdateRequestDTO() { };
            ViewBag.Topics = _topicService.GetTopN(20);
            return View(model);
        }

        [HttpPost, ModelValidatorFilter, Authorize]
        public int CreateOrUpdate(ArticleCreateOrUpdateRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.TopicName))
            {
                request.TopicName = "杂谈";
            }
            return _service.CreateOrUpdate(request);
        }

        [HttpPost, Authorize]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpPost, Authorize]
        public void Thumb(int id)
        {
            _thumbService.CreateOrRemove(id);
        }

        [HttpPost, Authorize]
        public async Task<ImageUploadResultDTO> UploadImage(ImageUploadRequestDTO request)
        {
            var arr = request.Base64.Split(";base64,", StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 2) throw new CodeException("无效的base64");
            var match = Regex.Match(arr[0], ".+image/(gif|jpeg|jpg|bmp|png)", RegexOptions.IgnoreCase);
            if(!match.Success) throw new CodeException("无效的图片格式");
            // 用户ID取16进制
            string virthPath = Path.Combine("upload", Convert.ToString(_identityUser.Id, 16), "imgs", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid().ToGuidString()}.{match.Groups[1].Value}");
            string absolutePath = Path.Combine(_hostEnvironment.WebRootPath, virthPath);
            string directory = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var bytes = Convert.FromBase64String(arr[1]);

            await System.IO.File.WriteAllBytesAsync(absolutePath, bytes);

            return new ImageUploadResultDTO
            {
                Path = "/" + virthPath.Replace("\\", "/"),
                Host = string.Empty
            };
        }
    }
}

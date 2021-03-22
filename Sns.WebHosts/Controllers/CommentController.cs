using Calamus.AspNetCore.FluentValidator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using Sns.Models;

namespace Sns.WebHosts.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _service;
        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpPost, ModelValidatorFilter]
        public IActionResult Submit(CommentCreateRequestDTO request, string returnUrl = "")
        {
            _service.Create(request);
            if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("list", "article");
        }

        [HttpPost]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpPost]
        public void ThumbupCreateOrRemove(int id)
        {
            _service.ThumbupCreateOrRemove(id);
        }
    }
}

using Calamus.AspNetCore.FluentValidator;
using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using Sns.Models;

namespace Sns.WebHosts.Controllers
{
    public class AboutController : Controller
    {
        private readonly IFeedbackService _service;
        public AboutController(IFeedbackService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Intro()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost, ModelValidatorFilter]
        public void Feedback(FeedbackCreateRequestDTO request)
        {
            _service.Create(request);
        }
    }
}

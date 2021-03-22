using Calamus.AspNetCore.Users;
using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using Sns.Models;

namespace Sns.WebHosts.Components
{
    public class MyHeadViewComponent : ViewComponent
    {
        private readonly IAccountService _service;
        private readonly IIdentityUser<int, LoginAuthModel> _identityUser;
        public MyHeadViewComponent(IAccountService service,
             IIdentityUser<int, LoginAuthModel> identityUser)
        {
            _service = service;
            _identityUser = identityUser;
        }
        public IViewComponentResult Invoke(AccountDetailResultDTO model = null)
        {
            model = model ?? _service.DetailOfMine(_identityUser.Id);
            return View(model);
        }
    }
}

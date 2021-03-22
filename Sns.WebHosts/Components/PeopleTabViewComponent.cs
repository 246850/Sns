using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Components
{
    public class PeopleTabViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

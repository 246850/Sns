using Calamus.Result;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Components
{
    public class AspNetCorePagerViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(PageModel model)
        {
            return View(model);
        }
    }
}

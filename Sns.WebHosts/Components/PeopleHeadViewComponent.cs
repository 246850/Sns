using Microsoft.AspNetCore.Mvc;
using Sns.IServices;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Components
{
    public class PeopleHeadViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(AccountDetailResultDTO model)
        {
            return View(model);
        }
    }
}

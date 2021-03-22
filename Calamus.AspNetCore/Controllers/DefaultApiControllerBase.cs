using Calamus.AspNetCore.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.AspNetCore.Controllers
{
    /// <summary>
    /// Api控制器基类
    /// </summary>
    [ApiController]
    [DefaultAuthorizeFilter]
    [Route("api/[controller]/[action]")]
    public abstract class DefaultApiControllerBase: ControllerBase
    {
    }
}

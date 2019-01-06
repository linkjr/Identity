using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.UserAuthenticationScheme)]
    public class BaseController : Controller
    {
    }
}
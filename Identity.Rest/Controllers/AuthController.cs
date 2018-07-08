using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Rest.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public abstract class AuthController : ApiController
    {
        public abstract IActionResult Token();
    }
}
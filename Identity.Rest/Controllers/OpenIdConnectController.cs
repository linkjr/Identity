using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Rest.Controllers
{
    public class OpenIdConnectController : AuthController
    {
        [HttpPost]
        public override IActionResult Token()
        {
            return Ok(new
            {
                result = true
            });
        }
    }
}
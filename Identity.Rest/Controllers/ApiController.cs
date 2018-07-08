using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Rest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
    }
}
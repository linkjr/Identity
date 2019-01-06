using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Areas.Authorization.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SignIn()
        {
            var identity = new ClaimsIdentity("Forms");
            identity.AddClaim(new Claim(ClaimTypes.Sid, "123"));
            await HttpContext.SignInAsync(AuthenticationSchemes.AdminAuthenticationScheme, new ClaimsPrincipal());

            return RedirectToAction("signout");
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.AdminAuthenticationScheme)]
        [HttpPost]
        public IActionResult SignOut()
        {
            Console.WriteLine($"IsAuthenticated:{User.Identity.IsAuthenticated}");
            Console.WriteLine($"AuthenticationType:{User.Identity.AuthenticationType}");
            return View();
        }
    }
}
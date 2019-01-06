using Identity.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Web.Controllers
{
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public IActionResult SignIn(string returnUrl)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            //定义声明标识，AuthenticationType必需设置，
            //否则登录后其它页面会挑战拒绝访问页面。
            var identity = new ClaimsIdentity("Forms");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "123"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "test"));
            await HttpContext.SignInAsync(AuthenticationSchemes.UserAuthenticationScheme, new ClaimsPrincipal(identity));
            return Redirect(returnUrl ?? "~/");
        }

        //[HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(AuthenticationSchemes.UserAuthenticationScheme);
            return RedirectToAction("index", "home");
        }
    }
}
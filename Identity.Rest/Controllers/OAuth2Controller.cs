using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Rest.Controllers
{
    /// <summary>
    /// 使用基于OAuth2.0标准认证方式认证。
    /// </summary>
    public class OAuth2Controller : AuthController
    {
        /// <summary>
        /// 获取OAuth2.0的令牌。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Token()
        {
            return Ok(new
            {
                result = true
            });
        }

        /// <summary>
        /// 通过OAuth2.0进行授权。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Authorize()
        {
            return Unauthorized();
        }

        /// <summary>
        /// 获取用户信息。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Graph()
        {
            return Ok();
        }
    }
}
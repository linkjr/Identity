using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Rest.Controllers
{
    /// <summary>
    /// 使用基于JWT标准认证方式认证。
    /// </summary>
    public class JwtController : AuthController
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 初始化 <see cref="ApiController"/> 类的构造函数。
        /// </summary>
        /// <param name="configuration"></param>
        public JwtController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// 获取JWT的令牌。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Token()
        {
            var payloadConfig = this.Configuration.GetSection("Jwt").GetSection("Payload");
            var key = Encoding.ASCII.GetBytes(payloadConfig["secret"]);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);
            var claims = new Claim[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, "一个主题"),  //sub has changed to nameidentifier.
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Gender ,"male"),
                    new Claim(ClaimTypes.GivenName ,"张三")
            };
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Issuer = payloadConfig["iss"],
            //    Audience = payloadConfig["aud"],
            //    Subject = new ClaimsIdentity(claims),
            //    IssuedAt = authTime,
            //    Expires = expiresAt,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            var tokenHandler = new JwtSecurityTokenHandler();
            //var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //var access_token = tokenHandler.WriteToken(securityToken);

            var jwtToken = new JwtSecurityToken(
                issuer: payloadConfig["iss"],
                audience: payloadConfig["aud"],
                claims: claims,
                notBefore: authTime,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));
            var access_token = tokenHandler.WriteToken(jwtToken);
            return Ok(new
            {
                token_type = JwtBearerDefaults.AuthenticationScheme,
                access_token,
                profile = new
                {
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }
    }
}
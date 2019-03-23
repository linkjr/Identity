using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            #region jwt authentication.

            var authenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            switch (authenticateScheme)
            {
                case CookieAuthenticationDefaults.AuthenticationScheme:
                    authenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    break;
                //case OAuthDefaults.DisplayName:
                //    authenticateScheme = OAuthDefaults.DisplayName;
                //    break;
                case OpenIdConnectDefaults.AuthenticationScheme:
                    authenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    break;
                case JwtBearerDefaults.AuthenticationScheme:
                    authenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    break;
                default:
                    authenticateScheme = OAuthDefaults.DisplayName;
                    break;
            }
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = authenticateScheme;
                options.DefaultChallengeScheme = authenticateScheme;
            })
            //.AddCookie(authenticateScheme, options =>
            // {

            // });
            //.AddOAuth(OAuthDefaults.DisplayName, options =>
            //{
            //    options.CallbackPath = "/api/oauth2/";
            //    options.ClientId = "test123";
            //    options.ClientSecret = "test";
            //    options.AuthorizationEndpoint = "/api/oauth2/authorize";
            //    options.TokenEndpoint = "/api/oauth2/token";
            //    options.UserInformationEndpoint = "/api/oauth2/graph";
            //})
             .AddJwtBearer(options =>
             {
                 //JwtBearer认证中，默认是通过Http的Authorization头来获取的，这也是最推荐的做法，但是在某些场景下，我们可能会使用Url或者是Cookie来传递Token
                 //http://www.cnblogs.com/RainingNight/p/jwtbearer-authentication-in-asp-net-core.html#自定义token获取方式
                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = async context =>
                     {
                         context.Token = context.Request.Query["access_token"];
                         await Task.CompletedTask;
                     }
                 };
                 //JwtBearer认证配置
                 //options.Authority = this.Configuration["oidc:authority"];
                 //options.Audience = this.Configuration["oidc:clientid"];
                 var key = System.Text.Encoding.ASCII.GetBytes(this.Configuration["jwt:payload:Secret"]);
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = this.Configuration["jwt:payload:iss"],

                     ValidAudience = this.Configuration["jwt:payload:aud"],

                     IssuerSigningKey = new SymmetricSecurityKey(key),


                     /***********************************TokenValidationParameters的参数默认值***********************************/
                     // RequireSignedTokens = true,
                     // SaveSigninToken = false,
                     // ValidateActor = false,
                     // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                     //ValidateAudience = true,
                     //ValidateIssuer = true,
                     // ValidateIssuerSigningKey = false,
                     // 是否要求Token的Claims中必须包含Expires
                     // RequireExpirationTime = true,
                     // 允许的服务器时间偏移量
                     // ClockSkew = TimeSpan.FromSeconds(300),
                     // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                     // ValidateLifetime = true
                 };
             });

            #endregion

            #region config swagger. name区分大小写，同下面的{v1}/swagger.json

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "HttpAuthentication Api"
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Identity.Rest.xml");
                options.IncludeXmlComments(xmlPath);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();


            app.UseSwagger(options => options.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value))
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = string.Empty;
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                });
        }
    }
}

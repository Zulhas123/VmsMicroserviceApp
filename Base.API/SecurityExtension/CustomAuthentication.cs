
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Reflection;
using System.Text;

namespace Base.API.SecurityExtension
{
    public class CustomAuthentication : AuthorizeAttribute
    {
        private readonly RequestDelegate next;
        private IConfiguration configuration;
        //IHttpContextAccessor httpContextAccessor;
        private JwtTokenHandler tokenHandler;
        public CustomAuthentication(RequestDelegate next)
        {

            this.next = next;

        }

        public async Task Invoke(HttpContext context, IConfiguration _configuration)
        {
            tokenHandler = new JwtTokenHandler();
            string token = context.Request.Headers[HeaderNames.Authorization];
            var routeValue = context.Request.GetDisplayUrl;
            var path = context.Request.Path;
            var host = context.Request.Host;
            
            //var controllerName = context.GetRouteValue("controller").ToString();
            //var actionName = context.GetRouteValue("action").ToString();
            bool hasAccess = tokenHandler.AttachAccountToContext(token, _configuration, host, path);

            if (hasAccess)
            {
                await this.next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }

           
            //  

        }
    }
    public static class MyCustomAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthentication>();
        }
    }
}
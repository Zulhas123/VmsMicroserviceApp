
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Reflection;
using System.Text;

namespace Base.API.SecurityExtension
{
    public class BasicAuthentication : AuthorizeAttribute
    {
        private readonly RequestDelegate next;
        private IConfiguration configuration;
        //IHttpContextAccessor httpContextAccessor;
        private JwtTokenHandler tokenHandler;
        public BasicAuthentication(RequestDelegate next)
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
            bool hasAccess = tokenHandler.CheckBasicAuthentication(token, _configuration);

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
    public static class MyBasicAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyBasicAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthentication>();
        }
    }
}
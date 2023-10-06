using Microsoft.AspNetCore.Builder;

namespace Base.API.SecurityExtension
{
    public class MyBasicAuthenticationMiddlewarePipeline
    {
       
        public void Configure(IApplicationBuilder app)
        {
            app.UseMyBasicAuthentication();
        }
    }
}

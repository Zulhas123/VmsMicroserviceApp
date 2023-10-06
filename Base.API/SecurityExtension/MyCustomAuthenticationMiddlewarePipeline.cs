using Microsoft.AspNetCore.Builder;

namespace Base.API.SecurityExtension
{
    public class MyCustomAuthenticationMiddlewarePipeline
    {
       
        public void Configure(IApplicationBuilder app)
        {
            app.UseMyCustomAuthentication();
        }
    }
}

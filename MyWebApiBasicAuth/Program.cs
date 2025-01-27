using Microsoft.AspNetCore.Authentication;
using MyWebApiBasicAuth.Auth;

namespace MyWebApiBasicAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddAuthentication("BasicAuthentication")
                //.AddScheme<AuthenticationSchemeOptions, BasicAuth>("BasicAuthentication", null);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            //app.UseAuthorization();
            //app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using MyWebApiBasicAuth.Auth;

namespace BmwFullProjectJquery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors();
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            //builder.Services.AddAuthentication("BasicAuthentication")
            //.AddScheme<AuthenticationSchemeOptions, BasicAuth>("BasicAuthentication", null);

            var app = builder.Build();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UseDefaultFiles();
            
            app.UseStaticFiles();
            
          // app.UseAuthentication();
            
            app.UseAuthorization();
            
            app.MapControllers();
            
            app.Run();
        }
    }
}
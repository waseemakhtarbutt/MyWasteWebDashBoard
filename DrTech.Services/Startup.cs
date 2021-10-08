using System.IO;
using System.Text;
using DrTech.Common.Extentions;
using DrTech.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services
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
            services.AddCors();

            services.AddMvc();
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
            });

            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.SECURITY_SECTION, Constants.AppSettings.SECURITY_ISSUER),
                ValidAudience = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.SECURITY_SECTION, Constants.AppSettings.SECURITY_AUDIENCE),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsHelper.GetAttributeValue(Constants.AppSettings.SECURITY_SECTION, Constants.AppSettings.SECURITY_KEY)))
            };


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwtConfig =>
                    {
                        jwtConfig.TokenValidationParameters = tokenParams;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            string Enviornment = AppSettingsHelper.GetEnvironmentValue(AppSettings.FILE_NAME, Constants.AppSettings.ENV);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(env.ContentRootPath + "\\images\\"),
                RequestPath = new PathString("/images")
            });
            if (Enviornment != Enviornemnt.CLOUD)
            {

                app.UseStaticFiles(); // For the wwwroot folder

                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                                        Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
                    RequestPath = new PathString("/Images")
                });

            }
            app.UseAuthentication();

            app.UseCors(builder => builder.
                     AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                   );
            app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=values}/{action}");

                //routes.MapRoute(name: "ImagesRoute",
                //    template: "Images/{filename}");
            });
        }
    }
}

using ECS.PR.Extra.Services;
using FRW.PR.Extra.Services;
using FRW.PR.Extra.Utils;
using FRW.TR.Contrats;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace FRW.PR.Extra
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
            services.AddSingleton<IVueParser>(new VueParser());

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.WithOrigins("https://infologique.net",
                                                          "http://127.0.0.1:8080",
                                                          "http://192.168.1.137:8080");
            });
            });

            services.AddControllers();
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });

            services.AddHttpClient<IDorsale, Dorsale>();
            services.AddHttpContextAccessor();
            services.AddScoped<FormulairesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {*/
                app.UseDeveloperExceptionPage();
           /* }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }*/
            var supportedCultures = new[] { "fr-CA", "en-CA" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseFileServer();

            app.UseRouting();
            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }


    }

    public class CustomerCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            //Go away and do a bunch of work to find out what culture we should do. 
            await Task.Yield();

            httpContext.Request.Query.TryGetValue("lang", out var lang);

            if (lang == string.Empty || lang == "fr")
            {
                //Return a provider culture result. 
                return new ProviderCultureResult("fr-CA");
            }
            else
            {
                //Return a provider culture result. 
                return new ProviderCultureResult("en-CA");
            }


            //In the event I can't work out what culture I should use. Return null. 
            //Code will fall to other providers in the list OR use the default. 
            //return null;
        }
    }
}

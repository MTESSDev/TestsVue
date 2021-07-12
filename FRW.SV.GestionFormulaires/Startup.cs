using System.Configuration;
using System.Threading.Tasks;
using FRW.SV.GestionFormulaires.SN;
using FRW.SV.GestionFormulaires.SN.ConversionDonnees;
using FRW.TR.Commun.Utils;
//using CAC.AccesDonnees.Dapper;
//using CAC.AccesProfil.Client;
//using ECS.AF.Session;
//using ECS.DO.ECS1;
//using ECS.SV.Helpers;
//using ECS.SV.Helpers.Extensions;
//using ECS.TR.Commun.Services;
//using ECS.TR.Contrats;
//using ECS.TR.Contrats.Contexte;
using MessagePack;
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartFormat;
using SV.GestionFormulaires.DAL;

namespace FRW.SV.GestionFormulaires
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
            Smart.Default.AddExtensions(new SmartFormatInclude());
            Smart.Default.Settings.FormatErrorAction = SmartFormat.Core.Settings.ErrorAction.Ignore;
            Smart.Default.Settings.ParseErrorAction = SmartFormat.Core.Settings.ErrorAction.Ignore;
            //services.AddDataProtection(options => options.ApplicationDiscriminator = "ECS")
            //  .AddKeyManagementOptions(options => options.XmlRepository = new ApiXmlRepository(Configuration));

            services.AddHttpContextAccessor();
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddScoped<ObtenirConfiguration>();
            services.AddScoped<DalFormulaires>();
            services.AddScoped<CreerFormulaireAF>();
            services.AddScoped<MajFormulaireAF>();
            services.AddScoped<ConvertirDonneesAF>();
            //services.AddScoped<ICodeNT, CodeNTAccesseur>();
            //services.AddScoped<IDALGeneriqueCAC, DalECS1>();
            //services.AddProfil(Configuration);
            //services.AddScoped<IContexte, ContexteSV>();
            //services.AddScoped<AuthentificationAF>();
            //services.AddScoped<SessionAF>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors",
                    builder =>
                    {
                        var origine = Configuration.GetValue<string?>("AllowedHostsCors");
                        origine = string.IsNullOrWhiteSpace(origine) ? "*" : origine;

                        if (origine.Equals("*"))
                        {
                            if (Configuration.GetValue<bool>("estProduction"))
                            {
                                throw new ConfigurationErrorsException("Doit spécifier une valeur différent de '*' pour la config 'AllowedHostsCors'.");
                            }

                            builder.SetIsOriginAllowed(_ => true);
                        }
                        else
                        {
                            builder.WithOrigins(origine.Split(";"));
                        }

                        builder
                            .WithMethods("GET", "POST")
                            .AllowCredentials()
                            .AllowAnyHeader();
                    });
            });

            var resolver = CompositeResolver.Create(NativeDateTimeResolver.Instance, ContractlessStandardResolver.Instance);
            var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
            MessagePackSerializer.DefaultOptions = options;

            services.AddControllers(option =>
            {
                //option.OutputFormatters.Clear();
                option.OutputFormatters.Add(new MessagePackOutputFormatter());
                //option.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new MvcOptions()));
                //option.InputFormatters.Clear();
                option.InputFormatters.Add(new MessagePackInputFormatter());
                //option.InputFormatters.Add(new SystemTextJsonInputFormatter(new MvcOptions()));

            });

            // Ajout d'un document 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FRW.SV.GestionFormulaires",
                    Version = "v1",
                    Description = "Service web pour la gestion des formulaires.",
                    Contact = new OpenApiContact() { Name = "Équipe DTN" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseEcsExceptionHandler();
            //app.UseMiddleware<NtlmAndAnonymousSetupMiddleware>();

            app.UseRouting();
            app.UseAuthorization();

            // Générateur Swagger
            app.UseSwagger(e => e.SerializeAsV2 = true);

            // Swagger-ui (HTML, JS, CSS, etc.), 
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECS.SV.Session");
                c.RoutePrefix = string.Empty; // Set Swagger UI at apps root
            });

            app.UseCors("AllowCors");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /*app.Run(context => {
                context.Response.Redirect("swagger/");
                return Task.CompletedTask;
            });*/
        }
    }
}

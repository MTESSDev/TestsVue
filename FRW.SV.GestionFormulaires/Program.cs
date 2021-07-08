//using ECS.TR.Commun.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog.Core;

namespace FRW.SV.GestionFormulaires
{
    public static class Program
    {
        public static LoggingLevelSwitch loggingLevelSwitch => new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Verbose);

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                //config.AddLegacyXmlConverter(hostingContext.HostingEnvironment);
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });//.UseEcsLog(loggingLevelSwitch, false);
    }
}

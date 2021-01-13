using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Parkner.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo cultura = CultureInfo.CreateSpecificCulture("es-MX");
            CultureInfo.DefaultThreadCurrentUICulture = cultura;
            CultureInfo.DefaultThreadCurrentCulture = cultura;

            Program.CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(l =>
                {
                    l.ClearProviders();
                    l.AddConsole();
                    l.AddTraceSource("Error, ActivityTracing");
                })
                .ConfigureWebHostDefaults(w => w.UseStartup<Startup>());
    }
}

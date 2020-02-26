using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AuditLog.Test
{

    /// <summary>
    /// მთავარი კლასი
    /// </summary>
    internal class Program
    {

        /// <summary>
        /// გამშვები ფუნქცია
        /// </summary>
        /// <param name="args">პარამეტრები</param>
        static void Main(string[] args)
        {
            // კესტრელ ჰოსტის გამზადება-დაკონფიგურირება-დაბილდვა
            var host = new WebHostBuilder()
                       .UseContentRoot(Directory.GetCurrentDirectory())
                       .ConfigureAppConfiguration((context, config) =>
                       {
                           var env = context.HostingEnvironment;
                           config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                       })
                       .UseKestrel()
                       .UseStartup<Startup>()
                       .ConfigureKestrel((context, options) =>
                       {
                       })
                       .UseUrls("http://0.0.0.0:4001")
                       .Build();
            // კესტრელ ჰოსტის გაშვება
            host.Run();
        }

    }

}

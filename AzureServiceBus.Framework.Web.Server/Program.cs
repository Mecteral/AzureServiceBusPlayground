using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AzureServiceBus.Framework.Web.Server
{
    public class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}
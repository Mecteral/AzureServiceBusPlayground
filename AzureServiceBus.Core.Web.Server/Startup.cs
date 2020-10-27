using System;
using System.Threading.Tasks;
using AzureServiceBus.Shared;
using AzureServiceBus.Shared.Consumer;
using AzureServiceBus.Shared.Models;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureServiceBus.Core.Web.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await QueuePublisher.SendToCoreQueue(new StringIntRequestModel
                    {
                        IntValue = 13,
                        StringValue = "FrameworkCall"
                    });

                    var response = await RpcPublisher.SendToCoreRpcQueue(new StringIntRequestModel
                    {
                        IntValue = 13,
                        StringValue = "FromFrameworkRpcCall"
                    });

                    Console.WriteLine(
                        $"int: {response.IntValue}, string: {response.StringValue}, time: {response.DateTime}");
                }
                catch (Exception e)
                {
                    
                }
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITest, Test>();

            services.AddMassTransit(collectionBusConfigurator =>
            {
                collectionBusConfigurator.AddConsumer<CoreStringIntRpcConsumer>();
                collectionBusConfigurator.AddConsumer<CoreStringIntConsumer>();
                collectionBusConfigurator.AddConsumer<CorrelationStartConsumer>();

                collectionBusConfigurator.UsingAzureServiceBus((context, configurator )=>
                {
                    AzureServiceBusFactory.ConfigureCoreWebServices(configurator, context);
                });
            });

            services.AddMassTransitHostedService();

        }
    }
}
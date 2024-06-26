namespace FileDownloaderAPI.Program;

using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

using FileDownloaderAPI.Consumer;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<FileUriConsumer>();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("localhost", "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });
                        cfg.ConfigureEndpoints(context);
                    });
                });
            });

    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}


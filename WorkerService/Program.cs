using Microsoft.Extensions.Hosting;
using WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

//using WorkerService;
//using Microsoft.Extensions.Hosting;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        CreateHostBuilder(args).Build().Run();
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .UseServiceBaseLifetime()
//            .ConfigureServices(services =>
//            {
//                services.AddHostedService<Worker>();
//            });
//}
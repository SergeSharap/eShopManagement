using LoggingService;
using LoggingService.Data;
using LoggingService.Repositories;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<MongoSettings>(context.Configuration.GetSection("MongoSettings"));
        services.AddSingleton<MongoDbContext>();
        services.AddSingleton<ILogRepository, LogRepository>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

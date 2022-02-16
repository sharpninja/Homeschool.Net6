
using Homeschool.Data.Context;

using Microsoft.EntityFrameworkCore;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(
        options =>
        {
            options.ServiceName = "Homeschool Service";
        }
    )
    .ConfigureAppConfiguration(
        builder =>
        {
            builder.AddJsonFile("appsettings.json", false);
            builder.AddUserSecrets(typeof(GradesService).Assembly, true);
        })
    .ConfigureLogging(builder => builder.AddConsole())
    .ConfigureServices(
        services =>
        {
            services
                .AddSingleton(
                    provider =>
                    {
                        ILogger<GradesService> logger  = provider.GetService<ILogger<GradesService>>()!;
                        HomeschoolContext context = provider.GetService<HomeschoolContext>()!;

                        return new GradesService(logger, context);
                    })
                .AddSingleton(
                    provider =>
                    {
                        string cs
                            = provider.GetRequiredService<IConfiguration>()["ConnectionString"];

                        return new DbContextOptionsBuilder<HomeschoolContext>()
                            .EnableDetailedErrors(true)
                            .UseSqlServer(
                                cs,
                                builder =>
                                {
                                }
                            )
                            .Options;
                    }
                )
                .AddDbContext<HomeschoolContext>()
                .AddServiceModelServices();

            services.AddHostedService(
                provider =>
                {
                    GradesService gradesService = provider.GetService<GradesService>()!;
                    ILogger<WindowsBackgroundService> logger = provider.GetService<ILogger<WindowsBackgroundService>>()!;

                    return new WindowsBackgroundService(gradesService, logger);
                });
        }
    )
    .Build();

await host.RunAsync();

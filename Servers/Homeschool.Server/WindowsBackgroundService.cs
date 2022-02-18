namespace Homeschool.Server;
using Data.Context;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Net;

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly GradesService _gradesService;
    private readonly ILogger<WindowsBackgroundService> _logger;

    public WindowsBackgroundService(
        GradesService gradesService,
        ILogger<WindowsBackgroundService> logger
    )
        => (_gradesService, _logger) = (gradesService, logger);

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        WebApplicationOptions webOptions = new()
        {
            ApplicationName = "Homeschool Services",
            ContentRootPath = Path.GetDirectoryName(typeof(GradesService).Assembly.Location),
            EnvironmentName = Debugger.IsAttached ? "Development" : "Production",
            WebRootPath = Path.GetDirectoryName(typeof(GradesService).Assembly.Location)
        };
        var builder = WebApplication.CreateBuilder(webOptions);
        builder.Host.UseWindowsService();
        builder.WebHost.ConfigureKestrel(
            static (context, options) =>
            {
                options.AllowSynchronousIO = true;
                options.ListenAnyIP(5000, listenOptions =>
                {
                    if (Debugger.IsAttached)
                    {
                        listenOptions.UseConnectionLogging();
                    }
                });
            });
        builder.Services.AddSingleton(
                provider =>
                {
                    string cs = provider.GetRequiredService<IConfiguration>()["ConnectionString"];

                    return new DbContextOptionsBuilder<HomeschoolContext>()
                        .EnableDetailedErrors(true)
                        .UseSqlServer(
                            cs,
                            builder => { }
                        )
                        .Options;
                }
            )
            .AddTransient<GradesService>()
            .AddDbContext<HomeschoolContext>()
            .AddServiceModelServices();

        var app = builder.Build();

        app.UseServiceModel(
            builder =>
                    {
                        builder.AddService<GradesService>()
                            .AddServiceEndpoint<GradesService, IGradesService>(
                                new BasicHttpBinding(),
                                "/GradesService/basichttp"
                            );
                    }
                );
        app.Run();

        return Task.CompletedTask;
    }
}
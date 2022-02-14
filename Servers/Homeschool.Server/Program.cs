
using Homeschool.Data.Context;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(
    static (context, options) =>
    {
        options.AllowSynchronousIO = true;
    }
);
builder.Services
    .AddSingleton(
    provider =>
    {
        string cs = provider.GetRequiredService<IConfiguration>()["ConnectionString"];

        return new DbContextOptionsBuilder<HomeschoolContext>().EnableDetailedErrors(true)
            .UseSqlServer(
                cs,
                builder =>
                {
                }
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

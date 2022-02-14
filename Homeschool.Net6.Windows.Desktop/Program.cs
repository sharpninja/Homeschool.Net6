using CommunityToolkit.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Homeschool.Net6;

using App;
using App.Views;

using Microsoft.Extensions.Logging;

using App = App.App;

public static class Program
{
    [ STAThread ]
    public static void Main(string[] args)
    {
        var builder = new WindowsAppSdkHostBuilder<Homeschool.App.App>();

        builder.ConfigureServices(
            (_, collection) =>
            {
                collection.AddLogging(
                    loggingBuilder =>
                    {
                        loggingBuilder.AddConsole()
                            .AddDebug()
                            .AddEventLog(
                                settings =>
                                {
                                    settings.Filter = static (s, level) => level==LogLevel.Error;
                                    settings.LogName = "Homeschool";
                                    settings.SourceName = "Homeschool";
                                }
                            );
                    }
                );
                // If your main Window is named differently, change it here.
                collection.AddSingleton<MainPage>()
                    .AddSingleton<MainWindow>()
                    .AddSingleton<ResearchPage>()
                    .AddSingleton<Studydotcom>();
            }
        );

        var app = builder.Build();

        App.Services = app.Services;

        app.StartAsync()
            .GetAwaiter()
            .GetResult();
    }
}
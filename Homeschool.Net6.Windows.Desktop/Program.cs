using CommunityToolkit.Extensions.Hosting;

namespace Homeschool.Net6;

using App;
using App.Views;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Proxy;

using App = App.App;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder =
            new WindowsAppSdkHostBuilder<Homeschool.App.App>();

        builder.ConfigureAppConfiguration(
            (context, configurationBuilder) =>
            {
                configurationBuilder.AddJsonFile("appsettings.json", false);
                configurationBuilder.AddUserSecrets(typeof(MainPage).Assembly, true);
            }
        );
        builder.ConfigureServices(
            (_, collection) =>
            {
                collection.AddLogging(
                    loggingBuilder =>
                    {
                        loggingBuilder//.AddConsole()
                            .AddDebug();
                    }
                );
                // If your main Window is named differently, change it here.
                collection
                    .AddSingleton<HomeschoolClientLogic>()
                    .AddSingleton<MainPage>(provider =>
                    {
                        var logger = provider.GetRequiredService<ILogger<MainPage>>();

                        var viewModel = provider.GetRequiredService<MainViewModel>();

                        try
                        {
                            logger.LogInformation("Creating MainPage");
                            var mainPage = new MainPage(logger, viewModel);


                            return mainPage;
                        }
                        catch (Exception ex)
                        {
                            if (logger is null)
                            {
                                throw new ApplicationException("Could not get ILogger<MainPage>");
                            }
                        }

                        return null;
                    })
                    .AddSingleton<MainWindow>(
                        provider =>
                        {
                            return new MainWindow(provider.GetRequiredService<MainPage>());
                        }
                    )
                    .AddSingleton<ResearchPage>()
                    .AddSingleton<HomePage>()
                    .AddSingleton<MainViewModel>()
                    .AddSingleton<Studydotcom>(
                        provider =>
                        {
                            return new Studydotcom(provider.GetRequiredService<StudydotcomViewModel>());
                        }
                    )
                    .AddSingleton<StudydotcomViewModel>();

                collection.AddTransient(
                    provider =>
                    {
                        Homeschool.Proxy.Proxy proxy = new();
                        proxy.Initialize(provider);

                        return proxy;
                    }
                );
            }
        );

        var host = builder.Build();

        App.Services = host.Services;

        host.StartAsync()
            .GetAwaiter()
            .GetResult();
    }
}
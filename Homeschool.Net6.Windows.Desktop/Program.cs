namespace Homeschool.Net6;

using App = Homeschool.App.App;

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
                configurationBuilder.AddJsonFile(
                    WcfSettings.GetSettingsFilename(
                        Windows.Storage.ApplicationData.Current.LocalFolder.Path),
                    true);
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
                    .AddSingleton<PathName>(
                        provider => new(Windows.Storage.ApplicationData.Current.LocalFolder.Path)
                    )
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

                        throw new ApplicationException("Could not create an instance of MainPage");
                    })
                    .AddSingleton<MainWindow>(
                        provider =>
                        {
                            return new MainWindow(provider.GetRequiredService<MainPage>());
                        }
                    )
                    .AddTransient<HomeschoolClientLogic>()
                    .AddTransient<WcfProxy>()
                    .AddTransient<WcfSettings>()
                    .AddSingleton<ResearchPage>()
                    .AddSingleton<SettingsViewModel>()
                    .AddSingleton<SettingsPage>()
                    .AddSingleton<HomePage>()
                    .AddSingleton<MainViewModel>()
                    .AddSingleton<Studydotcom>(
                        provider =>
                        {
                            return new Studydotcom(provider.GetRequiredService<StudydotcomViewModel>());
                        }
                    )
                    .AddSingleton<StudydotcomViewModel>();
            }
        );

        var host = builder.Build();

        App.Services = host.Services;

        host.StartAsync()
            .GetAwaiter()
            .GetResult();
    }
}

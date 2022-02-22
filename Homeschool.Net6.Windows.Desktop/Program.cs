namespace Homeschool.Net6;

using Aws.Client;

using CommunityToolkit.WinUI.Helpers;

using Newtonsoft.Json;

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
                //configurationBuilder.AddJsonFile(
                //    WcfSettings.GetSettingsFilename(
                //        Windows.Storage.ApplicationData.Current.LocalFolder.Path),
                //    true);
                configurationBuilder.AddUserSecrets(typeof(MainPage).Assembly, true);
                configurationBuilder.AddEnvironmentVariables();
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
                    .AddSingleton<ResearchPage>()
                    .AddSingleton<SettingsViewModel>(
                        provider =>
                        {
                            if (!ApplicationData.Current.LocalFolder
                                    .FileExistsAsync("settings.json")
                                    .GetAwaiter()
                                    .GetResult())
                            {
                                return new SettingsViewModel();
                            }

                            var jsonStream = ApplicationData.Current.LocalFolder
                                .OpenStreamForReadAsync("settings.json")
                                .GetAwaiter()
                                .GetResult();

                            var buffer = new byte[jsonStream.Length];
                            int read = jsonStream.Read(buffer, 0, buffer.Length);
                            jsonStream.Close();

                            var json = Encoding.UTF8.GetString(buffer);

                            if (json is not (null or ""))
                            {
                                return JsonConvert.DeserializeObject<SettingsViewModel>(
                                    json,
                                    MainPage.JsonOptions!
                                );
                            }

                            return new SettingsViewModel();
                        })
                    .AddSingleton<SettingsPage>()
                    .AddSingleton<HomePage>()
                    .AddSingleton<MainViewModel>()
                    .AddTransient<Lambdas>()
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

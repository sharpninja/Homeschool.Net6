namespace Homeschool.App;

    // ReSharper disable once RedundantUsingDirective
using System.Reflection;

using Helper;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Views;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public sealed partial class App    : Application
{
    private Window? _window;

    /// <summary>
    /// Initializes the singleton Application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        //Suspending += OnSuspending;
        //this.Resuming += App_Resuming;
        //RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;

        if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(
                "Windows.Foundation.UniversalApiContract",
                6
            ))
        {
            FocusVisualKind = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily ==
                              "Xbox"
                ? FocusVisualKind.Reveal
                : FocusVisualKind.HighVisibility;
        }
    }

    // ReSharper disable once UnusedMember.Global
    public void EnableSound(bool withSpatial = false)
    {
        ElementSoundPlayer.State = ElementSoundPlayerState.On;

        ElementSoundPlayer.SpatialAudioMode = !withSpatial
            ? ElementSpatialAudioMode.Off
            : ElementSpatialAudioMode.On;
    }

    public static TEnum GetEnum<TEnum>([ NotNull ] string text) where TEnum : struct
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (!System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(TEnum)).IsEnum)
        {
            throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
        }
        return (TEnum) Enum.Parse(typeof(TEnum), text);
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    // ReSharper disable once AsyncVoidMethod
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        //CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;

        await EnsureWindow(args).ConfigureAwait(false);
    }

    private void DebugSettings_BindingFailed(object sender, BindingFailedEventArgs e)
    {

    }

    private async Task EnsureWindow(LaunchActivatedEventArgs args)
    {
        if ((_window ??= Window.Current) is null)
        {
            var assembly = Assembly.GetEntryAssembly();
            var mainWindowType = assembly.GetType("Homeschool.Net6.MainWindow", true, true);
            _window = (Window)Services?.GetService(mainWindowType);
            _window?.Activate();
        }

        ThemeHelper._currentApplicationWindow = _window;

        ThemeHelper.Initialize();
    }

    // ReSharper disable once UnusedParameter.Local
    private static void UpdateNavigationBasedOnSelectedPage(Frame rootFrame)
    {
    }

    public static IServiceProvider? Services { get; set; }

    [SuppressMessage("Compatibility", "Uno0001:Uno type or member is not implemented", Justification = "<Pending>")]
    private Frame GetRootFrame()
    {
        Frame rootFrame=null;
        var windowContent = _window?.Content;
        if (windowContent is not MainPage rootPage)
        {
            rootPage = Services?.GetRequiredService<MainPage>() ?? new MainPage(Services.GetService<ILogger<MainPage>>());
            //rootFrame = rootPage.RootPane;
            rootFrame ??= new Frame();

            //SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
            rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            rootFrame.NavigationFailed += OnNavigationFailed;
        }
        else
        {
            rootFrame = rootPage.RootPane; //rootPage.RootPane;
        }

        return rootFrame;
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new("Failed to load Page " + e.SourcePageType.FullName);
    }
}

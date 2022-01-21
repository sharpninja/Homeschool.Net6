namespace Homeschool.App;

    // ReSharper disable once RedundantUsingDirective
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;

using Common;

using Views;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public sealed partial class App    : Application
{

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

    //private async void App_Resuming(object sender, object e)
    //{
    //    // We are being resumed, so lets restore our state!
    //    try
    //    {
    //        await SuspensionManager.RestoreAsync();
    //    }
    //    finally
    //    {
    //        switch (NavigationRootPage.RootFrame?.Content)
    //        {
    //            case ItemPage itemPage:
    //                itemPage.SetInitialVisuals();
    //                break;
    //            case NewControlsPage _:
    //            case AllControlsPage _:
    //                NavigationRootPage.Current.NavigationView.AlwaysShowHeader = false;
    //                break;
    //        }
    //    }

    //}

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    // ReSharper disable once AsyncVoidMethod
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
//#if DEBUG
//        //if (System.Diagnostics.Debugger.IsAttached)
//        //{
//        //    this.DebugSettings.EnableFrameRateCounter = true;
//        //}

//        if (System.Diagnostics.Debugger.IsAttached)
//        {
//            DebugSettings.BindingFailed += DebugSettings_BindingFailed;
//        }
//#endif

        //CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;

        await EnsureWindow(args).ConfigureAwait(false);
    }

    private void DebugSettings_BindingFailed(object sender, BindingFailedEventArgs e)
    {

    }

    // ReSharper disable once AsyncVoidMethod
    //protected override async void OnActivated(ActivationE args)
    //{
    //    await EnsureWindow(args).ConfigureAwait(false);

    //    base.OnActivated(args);
    //}

    private async Task EnsureWindow(LaunchActivatedEventArgs args)
    {
        // No matter what our destination is, we're going to need control data loaded - let's knock that out now.
        // We'll never need to do this again.
        //await ControlInfoDataSource.Instance.GetGroupsAsync();

        MainWindow m_window = new MainWindow();
        m_window.Activate();

        Frame rootFrame = GetRootFrame();

        //ThemeHelper.Initialize();

        Type targetPageType = typeof(ResearchPage);
        var targetPageArguments = string.Empty;

        rootFrame?.Navigate(targetPageType, targetPageArguments);

        // Ensure the current window is active
    }

    // ReSharper disable once UnusedParameter.Local
    private static void UpdateNavigationBasedOnSelectedPage(Frame rootFrame)
    {
    }

    [SuppressMessage("Compatibility", "Uno0001:Uno type or member is not implemented", Justification = "<Pending>")]
    private Frame GetRootFrame()
    {
        Frame rootFrame;
        if (Window.Current?.Content is not MainPage rootPage)
        {
            rootPage = new();
            rootFrame = rootPage.RootPane;
            if (rootFrame == null)
            {
                //throw new("Root frame not found");
                return null;
            }

            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
            rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            rootFrame.NavigationFailed += OnNavigationFailed;

            Window.Current.Content = rootPage;
        }
        else
        {
            rootFrame = rootPage.RootPane;
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

    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
        App.UpdateNavigationBasedOnSelectedPage(GetRootFrame());
        deferral.Complete();
    }
}

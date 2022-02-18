namespace Homeschool.App.Helper;

/// <summary>
/// Class providing functionality around switching and restoring theme wcfSettings
/// </summary>
public static class ThemeHelper
{
    private const string SELECTED_APP_THEME_KEY = "SelectedAppTheme";
    public static Window? _currentApplicationWindow;
    // Keep reference so it does not get optimized/garbage collected
    private static UISettings? _uiSettings;


    /// <summary>
    /// Gets or sets (with LocalSettings persistence) the RequestedTheme of the root element.
    /// </summary>
    public static ApplicationTheme RootTheme
    {
        get => App.Current.Window is not null
            ? App.Current.RequestedTheme
            : default;
        set
        {
            if (App.Current.Window?.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = Enum.TryParse(
                    value.ToString(),
                    true,
                    out ElementTheme theme
                )
                    ? theme
                    : ElementTheme.Default;
            }

            ApplicationData.Current.LocalSettings.Values[SELECTED_APP_THEME_KEY] = value.ToString();
        }
    }

    public static void Initialize()
    {
        // Save reference as this might be null when the user is in another app
        _currentApplicationWindow = App.Current.Window;
        // ReSharper disable once SuggestVarOrType_BuiltInTypes
        string? savedTheme = ApplicationData.Current.LocalSettings.Values[SELECTED_APP_THEME_KEY]
            ?.ToString();

        if (savedTheme is not null)
        {
            RootTheme = App.GetEnum<ApplicationTheme>(savedTheme);
        }

        // Registering to color changes, thus we notice when user changes theme system wide
        _uiSettings = new();
        _uiSettings.ColorValuesChanged += UiSettings_ColorValuesChanged;
    }

    private static void UiSettings_ColorValuesChanged(UISettings sender, object args)
    {
    }

    public static bool IsDarkTheme()
    {
        if (RootTheme.ToString() == ElementTheme.Default.ToString())
        {
            return Application.Current.RequestedTheme == ApplicationTheme.Dark;
        }
        return RootTheme.ToString() == ElementTheme.Dark.ToString();
    }
}
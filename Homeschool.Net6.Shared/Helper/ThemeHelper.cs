

//using App = Homeschool.App.App;

namespace Homeschool.App.Helper;


using Windows.Storage;
using Windows.UI.ViewManagement;

using Microsoft.UI;

/// <summary>
/// Class providing functionality around switching and restoring theme settings
/// </summary>
public static class ThemeHelper
{
    private const string SELECTED_APP_THEME_KEY = "SelectedAppTheme";
    public static Window _currentApplicationWindow;
    // Keep reference so it does not get optimized/garbage collected
    public static UISettings _uiSettings;


    /// <summary>
    /// Gets or sets (with LocalSettings persistence) the RequestedTheme of the root element.
    /// </summary>
    public static ElementTheme RootTheme
    {
        get
        {
            if (Window.Current.Content is FrameworkElement rootElement)
            {
                return rootElement.RequestedTheme;
            }

            return ElementTheme.Default;
        }
        set
        {
            if (Window.Current.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = value;
            }

            ApplicationData.Current.LocalSettings.Values[ThemeHelper.SELECTED_APP_THEME_KEY] = value.ToString();
            ThemeHelper.UpdateSystemCaptionButtonColors();
        }
    }

    public static void Initialize()
    {
        //// Save reference as this might be null when the user is in another app
        //ThemeHelper.CurrentApplicationWindow = Window.Current;
        //string savedTheme = ApplicationData.Current.LocalSettings.Values[ThemeHelper.SELECTED_APP_THEME_KEY]?.ToString();

        //if (savedTheme != null)
        //{
        //    ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(savedTheme);
        //}

        //// Registering to color changes, thus we notice when user changes theme system wide
        //ThemeHelper.UiSettings = new();
        //ThemeHelper.UiSettings.ColorValuesChanged += ThemeHelper.UiSettings_ColorValuesChanged;
    }

    private static void UiSettings_ColorValuesChanged(UISettings sender, object args)
    {
        // Make sure we have a reference to our window so we dispatch a UI change
        if (ThemeHelper._currentApplicationWindow != null)
        {
            // Dispatch on UI thread so that we have a current appbar to access and change
            ThemeHelper._currentApplicationWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                ThemeHelper.UpdateSystemCaptionButtonColors();
            });
        }
    }

    public static bool IsDarkTheme()
    {
        if (ThemeHelper.RootTheme == ElementTheme.Default)
        {
            return Application.Current.RequestedTheme == ApplicationTheme.Dark;
        }
        return ThemeHelper.RootTheme == ElementTheme.Dark;
    }

    public static void UpdateSystemCaptionButtonColors()
    {
        var titleBar = ApplicationView.GetForCurrentView().TitleBar;

        if (ThemeHelper.IsDarkTheme())
        {
            titleBar.ButtonForegroundColor = Colors.White;
        }
        else
        {
            titleBar.ButtonForegroundColor = Colors.Black;
        }
    }
}
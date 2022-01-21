namespace Homeschool.App.Helper;


using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.ViewManagement;

using Microsoft.UI;

public static class NavigationOrientationHelper
{

    private const string IS_LEFT_MODE_KEY = "NavigationIsOnLeftMode";

    public static bool IsLeftMode
    {
        get
        {
            object valueFromSettings = ApplicationData.Current.LocalSettings.Values[NavigationOrientationHelper.IS_LEFT_MODE_KEY];
            if(valueFromSettings == null)
            {
                ApplicationData.Current.LocalSettings.Values[NavigationOrientationHelper.IS_LEFT_MODE_KEY] = true;
                valueFromSettings = true;
            }
            return (bool)valueFromSettings;
        }

        set
        {
            NavigationOrientationHelper.UpdateTitleBar(value);
            ApplicationData.Current.LocalSettings.Values[NavigationOrientationHelper.IS_LEFT_MODE_KEY] = value;
        }
    }

    public static void UpdateTitleBar(bool isLeftMode)
    {
        CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = isLeftMode;

        ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

        if (isLeftMode)
        {
            //NavigationRootPage.Current.NavigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Auto;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
        else
        {
            //NavigationRootPage.Current.NavigationView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top;
            UISettings userSettings = new();
            titleBar.ButtonBackgroundColor = userSettings.GetColorValue(UIColorType.Accent);
            titleBar.ButtonInactiveBackgroundColor = userSettings.GetColorValue(UIColorType.Accent);
        }
    }

}
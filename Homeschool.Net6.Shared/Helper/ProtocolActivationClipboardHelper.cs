namespace Homeschool.App.Helper;

using System;

using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

using Data;

/// <summary>
/// Class providing functionality to support generating and copying protocol activation URIs.
/// </summary>
public static class ProtocolActivationClipboardHelper
{
    private const string SHOW_COPY_LINK_TEACHING_TIP_KEY = "ShowCopyLinkTeachingTip";

    public static bool ShowCopyLinkTeachingTip
    {
        get
        {
            var valueFromSettings = ApplicationData.Current.LocalSettings.Values[ProtocolActivationClipboardHelper.SHOW_COPY_LINK_TEACHING_TIP_KEY];
            if (valueFromSettings == null)
            {
                ApplicationData.Current.LocalSettings.Values[ProtocolActivationClipboardHelper.SHOW_COPY_LINK_TEACHING_TIP_KEY] = true;
                valueFromSettings = true;
            }
            return (bool)valueFromSettings;
        }

        set => ApplicationData.Current.LocalSettings.Values[ProtocolActivationClipboardHelper.SHOW_COPY_LINK_TEACHING_TIP_KEY] = value;
    }

    public static void Copy(ControlInfoDataItem item)
    {
        Uri uri = new($"xamlcontrolsgallery://item/{item.UniqueId}", UriKind.Absolute);
        ProtocolActivationClipboardHelper.Copy(uri, $"{Package.Current.DisplayName} - {item.Title} Sample");
    }

    public static void Copy(ControlInfoDataGroup group)
    {
        Uri uri = new($"xamlcontrolsgallery://category/{group.UniqueId}", UriKind.Absolute);
        ProtocolActivationClipboardHelper.Copy(uri, $"{Package.Current.DisplayName} - {group.Title} Samples");
    }

    private static void Copy(Uri uri, string displayName)
    {
        var htmlFormat = HtmlFormatHelper.CreateHtmlFormat($"<a href='{uri}'>{displayName}</a>");

        DataPackage dataPackage = new();
        dataPackage.SetApplicationLink(uri);
        dataPackage.SetWebLink(uri);
        dataPackage.SetText(uri.ToString());
        dataPackage.SetHtmlFormat(htmlFormat);

        Clipboard.SetContentWithOptions(dataPackage, null);
    }
}
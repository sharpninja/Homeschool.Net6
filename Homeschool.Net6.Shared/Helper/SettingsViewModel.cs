namespace Homeschool.App.Helper;

using CommunityToolkit.Mvvm.ComponentModel;

[ObservableObject]
public partial class SettingsViewModel
{
    [ObservableProperty]
    private bool _isScreenshotMode;

    public SettingsViewModel()
    {
        try
        {
            _screenshotStorageFolder = ApplicationData.Current.LocalFolder;
        }
        catch
        {
            // ignore
        }
    }

    private StorageFolder? _screenshotStorageFolder;

    public StorageFolder ScreenshotStorageFolder => _screenshotStorageFolder;

    public string ScreenshotStorageFolderPath
    {
        get => _screenshotStorageFolder.Path;
        set
        {
            if (_screenshotStorageFolder.Path == value)
            {
                return;
            }
            _screenshotStorageFolder = StorageFolder.GetFolderFromPathAsync(value)
                .GetAwaiter()
                .GetResult();

            OnPropertyChanged();
            OnPropertyChanged(nameof(ScreenshotStorageFolder));
        }
    }
}

namespace Homeschool.App.Helper;

using CommunityToolkit.Mvvm.ComponentModel;

using Newtonsoft.Json;

[ObservableObject]
public partial class SettingsViewModel
{
    [ ObservableProperty, JsonProperty ]
    public string _maxLessons = "6";

    [ObservableProperty, JsonProperty ]
    public bool _isScreenshotMode;

    [JsonConstructor]
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

    [ICommand]
    private void SetMaxLessons(string maxLessons)
    {
        MaxLessons = maxLessons;
    }

    [ JsonIgnore ]
    private StorageFolder? _screenshotStorageFolder;

    [JsonIgnore]
    public StorageFolder ScreenshotStorageFolder => _screenshotStorageFolder;

    [JsonProperty]
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

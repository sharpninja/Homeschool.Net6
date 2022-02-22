namespace Homeschool.App;

using Newtonsoft.Json;

[ObservableObject]
public partial class MainViewModel
{
    public SettingsViewModel Settings
    {
        get;
    }

    private static MainViewModel? _instance = null;

    [ObservableProperty]
    protected string? status;

    public MainViewModel(SettingsViewModel settings)
    {
        Settings = settings;
        MainViewModel._instance = this;

        settings.PropertyChanged += SettingsChanged;
    }

    private void SettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        var json = JsonConvert.SerializeObject(Settings, MainPage.JsonOptions);

        var file = ApplicationData.Current.LocalFolder.CreateFileAsync(
            "settings.json",
            CreationCollisionOption.ReplaceExisting
        ).GetAwaiter().GetResult();

        var bytes = Encoding.UTF8.GetBytes(json);

        var stream = file.OpenStreamForWriteAsync()
            .GetAwaiter()
            .GetResult();

        stream.Write(bytes, 0, bytes.Length);

        stream.Flush();
        stream.Close();

        SetStatus($"Updated settings file.");
    }

    public static void SetStatus(string newStatus)
        => MainViewModel._instance!.Status = $"{DateTime.Now:t}: {newStatus}";
}

namespace Homeschool.App.Controls;

using System;
using System.Collections.Concurrent;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


public class BrowserViewModel : ObservableObject
{
    private RelayCommand? _goForward;
    private RelayCommand? _goBack;
    private RelayCommand<string>? _go;
    private string _url = "https://study.com";
    private readonly ConcurrentStack<Uri?> _history = new();
    private readonly ConcurrentStack<Uri?> _forward = new();

    public string Url
    {
        get => _url;
        set => SetProperty(ref _url, value);
    }

    public RelayCommand<string> Go => _go ??= new(s =>
    {
        var address = s ?? "";

        Forward.Clear();

        var url = address.Split(':').First().ToLower() switch
        {
            "http" or "https" => s,
            _ => $"https://bing.com/search/?q={s}",
        };

        NavigateToUrl?.Invoke(url);
    }, s => s is not (null or ""));

    public RelayCommand GoBack => _goBack ??= new(() =>
    {
        if (!History.TryPop(out var url))
        {
            return;
        }

        Forward.Push(new(Url));
        NavigateToUrl?.Invoke(url.ToString());
    }, () => History.Count > 0);

    public RelayCommand GoForward => _goForward ??= new(() =>
    {
        if (!Forward.TryPop(out var url))
        {
            return;
        }

        NavigateToUrl?.Invoke(url?.ToString());
    }, () => Forward.Count > 0);

    public event Action<string?>? NavigateToUrl;

    public bool CanGoBack => History.Count > 0;
    public bool CanGoForward => Forward.Count > 0;

    public new void OnPropertyChanged(string propertyName)
    {
        base.OnPropertyChanged(propertyName);
    }

    public ConcurrentStack<Uri?> History => _history;

    public ConcurrentStack<Uri?> Forward => _forward;
}
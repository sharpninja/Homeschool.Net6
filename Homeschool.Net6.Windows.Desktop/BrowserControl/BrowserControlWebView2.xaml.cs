#pragma warning disable CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.
namespace Homeschool.Net6.BrowserControl;

using System.Diagnostics;

using Windows.System;
using Windows.UI.Core;

using App.Controls;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

public sealed partial class BrowserControlWebView2 : IBrowserControlPage
{
    public BrowserViewModel ViewModel { get; set; }
    public BrowserControlWebView2()
    {
        ViewModel = new BrowserViewModel();
        InitializeComponent();

        Loaded += BrowserControl_Loaded;
    }

    private DispatcherQueue GetCurrentDispatcherQueue()
        => Windows.System.DispatcherQueue.GetForCurrentThread();

    private async void BrowserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await Browser.EnsureCoreWebView2Async().AsTask().ConfigureAwait(false);

        _ = GetCurrentDispatcherQueue() is not null
            ? GetCurrentDispatcherQueue().TryEnqueue(EventHandlers)
            : await Dispatcher.TryRunAsync(
                CoreDispatcherPriority.Normal,
                EventHandlers);

        void EventHandlers()
        {
            Browser.NavigationStarting += Browser_NavigationStarting;
            Browser.WebMessageReceived += Browser_WebMessageReceived;
            Browser.NavigationCompleted += Browser_NavigationCompleted;

            Browser.CoreWebView2.NewWindowRequested += NewWindowRequested;

            ViewModel.NavigateToUrl += url => Browser.Source = new Uri(url);
        }
    }

    private void NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        e.NewWindow = Browser.CoreWebView2;
    }

    private void Browser_WebMessageReceived(WebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
    {
        Debug.WriteLine(args.WebMessageAsJson);
    }

    private void Browser_NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        ViewModel.History.Push(new Uri(ViewModel.Url));
        ViewModel.GoBack.NotifyCanExecuteChanged();
        ViewModel.OnPropertyChanged(nameof(ViewModel.CanGoBack));
        ViewModel.GoForward.NotifyCanExecuteChanged();
    }

    private void Browser_NavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs args)
    {
        ViewModel.Url = args.Uri;
    }
}
#pragma warning restore CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.

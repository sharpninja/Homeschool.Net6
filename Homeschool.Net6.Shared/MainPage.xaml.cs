

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Homeschool.App;

using System;

using Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage
{
    public ILogger<MainPage> Logger
    {
        get;
    }

    public IBrowserControlPage? Browser { get; set; }
    public BrowserViewModel ViewModel => Browser?.ViewModel;

    public MainPage(ILogger<MainPage> logger)
    {
        Logger = logger;
        InitializeComponent();
    }

    public void Frame_Navigated(object sender, NavigationEventArgs args)
    {
    }

    public void NavItem_Invoked(object sender, NavigationViewItemInvokedEventArgs args)
    {
        Logger.LogInformation($"args: {args}");

        Logger.LogInformation($"args.InvokedItem: {args.InvokedItem}");

        Logger.LogInformation($"args.IsSettingsInvoked: {args.IsSettingsInvoked}");

        Logger.LogInformation($"args.InvokedItemContainer: {args.InvokedItemContainer}");

        if (args.InvokedItemContainer is not NavigationViewItem viewItem)
        {
            return;
        }

        FrameworkElement? target = viewItem.Tag switch
        {
            nameof(Studydotcom) => App.Services.GetRequiredService<Studydotcom>(),
            nameof(ResearchPage) => App.Services.GetRequiredService<ResearchPage>(),
            _ => null
        };

        if (target is null)
        {
            return;
        }

        RootPane.Content = target;
        NavView.HeaderTemplate = (DataTemplate)target.Resources["AppBarTemplate"];
    }
}
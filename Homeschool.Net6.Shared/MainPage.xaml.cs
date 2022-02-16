namespace Homeschool.App;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed partial class MainPage : Page
{
    public ILogger<MainPage> Logger
    {
        get;
        private set;
    }

    //public IBrowserControlPage? Browser { get; set; }
    //public BrowserViewModel? ViewModel => Browser?.ViewModel;

    public MainPage(ILogger<MainPage> logger, MainViewModel viewModel)
    {
        Logger = logger;
        ViewModel = viewModel;
        Logger.LogInformation("MainPage.ctor(): Entered.");
        InitializeComponent();

        this.Loaded += (sender, args) =>
        {
            NavigationViewItem study = NavView.MenuItems.FirstOrDefault() as NavigationViewItem;

            Logger.LogInformation($"{study}");

            NavigateTo(study);
        };
        //NavView.
        Logger.LogInformation("MainPage.ctor(): Left.");
    }

    public MainViewModel ViewModel
    {
        get;
        set;
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
    }

    private void NavigateTo(NavigationViewItem viewItem)
    {
        FrameworkElement? target = viewItem.Tag switch
        {
            nameof(Studydotcom) => App.Services!.GetRequiredService<Studydotcom>(),
            nameof(ResearchPage) => App.Services!.GetRequiredService<ResearchPage>(),
            nameof(HomePage) => App.Services!.GetRequiredService<HomePage>(),
            _ => null
        };

        if (target is null)
        {
            return;
        }

        RootPane.Content = target;
        //var resource = target.Resources["AppBarTemplate"];

        //if (resource is AppBar header)
        //{
        //    this.As<Page>().TopAppBar = header;
        //}
    }
}

[ObservableObject]
public partial class MainViewModel
{
    private static MainViewModel? _instance = null;

    [ObservableProperty]
    protected string? status;

    public MainViewModel()
        => _instance = this;

    public static void SetStatus(string newStatus)
        => _instance!.Status = $"{DateTime.Now:t}: {newStatus}";
}
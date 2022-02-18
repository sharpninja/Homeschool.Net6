namespace Homeschool.App;

using CommunityToolkit.Mvvm.ComponentModel;

using Newtonsoft.Json;

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

    private Newtonsoft.Json.JsonSerializerSettings Options { get; } = new()
    {
        MaxDepth = 2,
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Include
    };

    public void NavItem_Invoked(object sender, NavigationViewItemInvokedEventArgs args)
    {
        Logger.LogInformation($"NavItem_Invoked: Entered.");

        //var json = JsonConvert.SerializeObject(args, Options);

        //Logger.LogInformation($"args:\n{json}");

        if (args.InvokedItemContainer is NavigationViewItem viewItem)
        {
            NavigateTo(viewItem);
        }
    }

    private void NavigateTo(NavigationViewItem viewItem)
    {
        FrameworkElement? target = viewItem.Tag switch
        {
            nameof(Studydotcom) => App.Services!.GetRequiredService<Studydotcom>(),
            nameof(ResearchPage) => App.Services!.GetRequiredService<ResearchPage>(),
            nameof(HomePage) => App.Services!.GetRequiredService<HomePage>(),
            _ => App.Services!.GetRequiredService<SettingsPage>()
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
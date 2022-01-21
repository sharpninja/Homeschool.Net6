

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Homeschool.App;

using System;

using Controls;



/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage    : Page
{
    public IBrowserControlPage Browser { get; set; }
    public BrowserViewModel ViewModel => Browser.ViewModel;
    private Type _browserType = null;

    public Frame RootPane { get; set; }

    public static event Func<Type> GetWebViewType;

    public MainPage()
    {
        InitializeComponent();

        //RootPane = rootFrame;

#if __WPF__
            _browserType = typeof(BrowserControlWebViewWpf); /* GetWebViewType != null
                ? GetWebViewType()
                : null;*/
#elif __WASM__
        _browserType = typeof(WasmBrowserControlWebView);
#elif WINDOWS10_0_22000_0_OR_GREATER
            _browserType = typeof(Net6.BrowserControl.BrowserControlWebView2);
#elif ANDROID
            _browserType = typeof(MobileBrowserControlWebView);
#else
            _browserType = typeof(Net6.BrowserControl.BrowserControlWebView2);
#endif

        //webFrame.Navigate(_browserType);

        //Console.WriteLine($"_browserType.FullName: [{_browserType.FullName}]");

        //Browser = webFrame.Content as IBrowserControlPage;

        //Browser.ViewModel.Url = "https://study.com";
    }


    private void OnRootFrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        if(e.SourcePageType == typeof(MainPage))
        {
            e.Cancel = true;
            return;
        }
    }

    private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
    {
        // Close any open teaching tips before navigation
        //CloseTeachingTips();

        //if (e.SourcePageType == typeof(AllControlsPage) ||
        //    e.SourcePageType == typeof(NewControlsPage))
        //{
        //    NavigationViewControl.AlwaysShowHeader = false;
        //}
        //else
        //{
        //    NavigationViewControl.AlwaysShowHeader = true;
        //}
    }

    private void TwoPaneView_ModeChanged(TwoPaneView sender, object args)
    {
        // Remove details content from it's parent panel.
        //((Panel)DetailsContent.Parent).Children.Remove(DetailsContent);
        // Set Normal visual state.
        VisualStateManager.GoToState(this, "Normal", true);

        // Single pane
        if (sender.Mode == TwoPaneViewMode.SinglePane)
        {
            // Add the details content to Pane1.
            //Pane1StackPanel.Children.Add(DetailsContent);
        }
        // Dual pane.
        else
        {
            // Put details content in Pane2.
            //Pane2Root.Children.Add(DetailsContent);

            // If also in Wide mode, set Wide visual state
            // to constrain the width of the image to 2*.
            if (sender.Mode == TwoPaneViewMode.Wide)
            {
                VisualStateManager.GoToState(this, "Wide", true);
            }
        }
    }
}
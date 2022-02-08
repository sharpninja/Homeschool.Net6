

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Homeschool.App;

using System;

using Controls;



/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage    : Page
{
    public IBrowserControlPage? Browser { get; set; }
    public BrowserViewModel ViewModel => Browser?.ViewModel;

    public Frame RootPane { get; set; }

    public MainPage()
    {
        InitializeComponent();

    }


}
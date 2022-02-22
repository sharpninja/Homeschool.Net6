

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Homeschool.App.Views;

//using CommunityToolkit.Mvvm.Input;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Studydotcom : Page
{
    public StudydotcomViewModel ViewModel
    {
        get;
    }

    public Studydotcom(StudydotcomViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        Loaded += (sender, args) =>
        {
            ViewModel.LoadLessonQueueAsync();
        };
    }

    public void TodaysWork_Click(object sender, RoutedEventArgs args)
    {
        ViewModel.LoadLessonQueueAsync();
    }

    private void Hyperlink_Click(Microsoft.UI.Xaml.Documents.Hyperlink sender, Microsoft.UI.Xaml.Documents.HyperlinkClickEventArgs args)
    {
        ViewModel.MarkOpened();
    }
}

//public partial class StudydotcomCommands
//{
//    [ICommand()]
//    public void NavigateTo()
//    {

//    }
//}
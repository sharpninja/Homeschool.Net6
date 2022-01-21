namespace Homeschool.App.Behaviors;

using System.Linq;

using Windows.UI;

using Helper;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

public partial class ImageScrollBehavior : DependencyObject, IBehavior
{
    private const int OPACITY_MAX_VALUE = 250;
    private const int ALPHA = 255;
    private const int MAX_FONT_SIZE = 28;
    private const int MIN_FONT_SIZE = 10;
    private const int SCROLL_VIEWER_THRESHOLD_VALUE = 85;

    private ScrollViewer _scrollViewer;
    private ListViewBase _listGridView;

    public DependencyObject AssociatedObject { get; private set; }

    public Control _targetControl
    {
        get => (Control)GetValue(ImageScrollBehavior._targetControlProperty);
        set => SetValue(ImageScrollBehavior._targetControlProperty, value);
    }

    // Using a DependencyProperty as the backing store for TargetControl.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty _targetControlProperty =
        DependencyProperty.Register("_targetControl", typeof(Control), typeof(ImageScrollBehavior), new PropertyMetadata(null));

    public void Attach(DependencyObject associatedObject)
    {
        AssociatedObject = associatedObject;
        if (!GetScrollViewer())
        {
            ((ListViewBase)associatedObject).Loaded += ListGridView_Loaded;
        }
    }

    private void ListGridView_Loaded(object sender, RoutedEventArgs e)
    {
        GetScrollViewer();
        _listGridView = sender as ListViewBase;
    }

    private bool GetScrollViewer()
    {
        _scrollViewer = UiHelper.GetDescendantsOfType<ScrollViewer>(AssociatedObject).FirstOrDefault();
        if (_scrollViewer != null)
        {
            _scrollViewer.ViewChanging += ScrollViewer_ViewChanging;
            return true;
        }
        return false;
    }

    private void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
    {
        double verticalOffset = ((ScrollViewer)sender).VerticalOffset;
        Control header = _targetControl;
        //header.BackgroundColorOpacity = verticalOffset / _opacityMaxValue;
        //header.AcrylicOpacity = 0.3 * (1 - (verticalOffset / _opacityMaxValue));
        if (verticalOffset < 10)
        {
            VisualStateManager.GoToState(header, "DefaultForeground", false);
            //header.BackgroundColorOpacity = 0;
            header.FontSize = 28;
            //header.AcrylicOpacity = 0.3;
        }
        else if (verticalOffset > ImageScrollBehavior.SCROLL_VIEWER_THRESHOLD_VALUE)
        {
            VisualStateManager.GoToState(header, "AlternateForeground", false);
            header.FontSize = ImageScrollBehavior.MIN_FONT_SIZE;
        }
        else
        {
            if (ThemeHelper.ActualTheme != ElementTheme.Dark)
            {
                VisualStateManager.GoToState(header, "DefaultForeground", false);
                Color foreground = new() { A = (byte)((verticalOffset > ImageScrollBehavior.SCROLL_VIEWER_THRESHOLD_VALUE) ? 0 : (ImageScrollBehavior.ALPHA * (1 - (verticalOffset / ImageScrollBehavior.SCROLL_VIEWER_THRESHOLD_VALUE)))) };
                foreground.R = foreground.G = foreground.B = 0;
                header.Foreground = new SolidColorBrush(foreground);
            }
            else
            {
                VisualStateManager.GoToState(header, "AlternateForeground", false);
            }

            header.FontSize = -(((verticalOffset / ImageScrollBehavior.SCROLL_VIEWER_THRESHOLD_VALUE) * (ImageScrollBehavior.MAX_FONT_SIZE - ImageScrollBehavior.MIN_FONT_SIZE)) - ImageScrollBehavior.MAX_FONT_SIZE);
        }
    }

    public void Detach()
    {
        ((ListViewBase)AssociatedObject).Loaded -= ListGridView_Loaded;
        AssociatedObject = null;
    }
}
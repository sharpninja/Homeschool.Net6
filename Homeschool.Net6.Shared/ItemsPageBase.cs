//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

namespace Homeschool.App;

using Data;

public abstract partial class ItemsPageBase : Page, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string? _itemId;
    private IEnumerable<ControlInfoDataItem>? _items;

    public IEnumerable<ControlInfoDataItem>? Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    /// <summary>
    /// Gets a value indicating whether the application's view is currently in "narrow" mode - i.e. on a mobile-ish device.
    /// </summary>
    protected virtual bool GetIsNarrowLayoutState()
        => throw new NotImplementedException();

    protected void OnItemGridViewContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        if (sender.ContainerFromItem(sender.Items.LastOrDefault()) is GridViewItem container)
        {
            container.XYFocusDown = container;
        }
    }

    protected void OnItemGridViewItemClick(object sender, ItemClickEventArgs e)
    {
        GridView gridView = (GridView)sender;
        ControlInfoDataItem item = (ControlInfoDataItem)e.ClickedItem;

        _itemId = item.UniqueId;

        Frame.Navigate(typeof(ItemPage), _itemId, new DrillInNavigationTransitionInfo());
    }

    protected void OnItemGridViewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Up)
        {
            DependencyObject nextElement = FocusManager.FindNextElement(FocusNavigationDirection.Up);
            if ((nextElement != null
                    ? nextElement.GetType()
                    : null) == typeof(Microsoft.UI.Xaml.Controls.NavigationViewItem))
            {
                //NavigationRootPage.Current.PageHeader.Focus(FocusState.Programmatic);
            }
            else
            {
                FocusManager.TryMoveFocus(FocusNavigationDirection.Up);
            }
        }
    }

    protected void OnItemGridViewLoaded(object sender, RoutedEventArgs e)
    {
        if (_itemId == null)
        {
            return;
        }

        GridView gridView = (GridView)sender;

        ControlInfoDataItem? item = gridView.ItemsSource is IEnumerable<ControlInfoDataItem> items
            ? items.FirstOrDefault(s => s.UniqueId == _itemId)
            : null;

        if (item != null)
        {
            gridView.ScrollIntoView(item);

            //if (NavigationRootPage.Current.IsFocusSupported)
            //{
            //    ((GridViewItem)gridView.ContainerFromItem(item))?.Focus(FocusState.Programmatic);
            //}
        }
    }

    protected void OnItemGridViewSizeChanged(object sender, SizeChangedEventArgs e)
    {
        GridView gridView = (GridView)sender;

        if (gridView.ItemsPanelRoot is ItemsWrapGrid wrapGrid)
        {
            if (GetIsNarrowLayoutState())
            {
                double wrapGridPadding = 88;
                wrapGrid.HorizontalAlignment = HorizontalAlignment.Center;

                wrapGrid.ItemWidth = gridView.ActualWidth - gridView.Padding.Left - gridView.Padding.Right - wrapGridPadding;
            }
            else
            {
                wrapGrid.HorizontalAlignment = HorizontalAlignment.Left;
                wrapGrid.ItemWidth = double.NaN;
            }
        }
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (object.Equals(storage, value))
        {
            return false;
        }

        storage = value;
        NotifyPropertyChanged(propertyName);
        return true;
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}
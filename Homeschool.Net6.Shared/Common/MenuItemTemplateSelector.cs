namespace Homeschool.App.Common;

[ContentProperty(Name = "ItemTemplate")]
class MenuItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate ItemTemplate { get; set; }

    //public string PaneTitle { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
        => item is Separator ? _separatorTemplate : item is Header ? _headerTemplate : ItemTemplate;

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        => item is Separator ? _separatorTemplate : item is Header ? _headerTemplate : ItemTemplate;

    internal DataTemplate _headerTemplate = (DataTemplate)XamlReader.Load(
        @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                   <NavigationViewItemHeader Content='{Binding Name}' />
                  </DataTemplate>");

    internal DataTemplate _separatorTemplate = (DataTemplate)XamlReader.Load(
        @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                    <NavigationViewItemSeparator />
                  </DataTemplate>");
}
namespace Homeschool.App.Common;

using System;

public class NullToVisibilityConverter : IValueConverter
{
    public Visibility NullValue { get; set; } = Visibility.Collapsed;
    public Visibility NonNullValue { get; set; } = Visibility.Visible;


    public object Convert(object value, Type targetType, object parameter, string language)
        => (value == null) ? NullValue : NonNullValue;

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotImplementedException();
}
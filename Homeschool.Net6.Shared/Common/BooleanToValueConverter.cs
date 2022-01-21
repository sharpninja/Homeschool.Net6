namespace Homeschool.App.Common;

using System;

public sealed class BooleanToValueConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
        => ((bool)value) ? parameter : null;

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotImplementedException();
}
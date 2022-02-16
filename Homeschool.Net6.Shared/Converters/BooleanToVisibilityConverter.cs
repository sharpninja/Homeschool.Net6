namespace Homeschool.App.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language
    )
    {
        try
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (value is null)
            {
                return Visibility.Collapsed;
            }

            if (targetType is
                {
                    Name: nameof(Visibility),
                } &&
                value is bool b)
            {
                return b
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }
        catch
        {
            return Visibility.Collapsed;
        }
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        string language
    )
        => throw new NotImplementedException();
}

public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language
    )
    {
        Visibility notNullVisibility = Visibility.Collapsed;
        Visibility nullVisibility = Visibility.Visible;

        try
        {
            if (parameter is string s)
            {
                (notNullVisibility, nullVisibility) = Enum.Parse<Visibility>(s) switch
                {
                    Visibility.Visible => (Visibility.Collapsed, Visibility.Visible),
                    Visibility.Collapsed => (Visibility.Visible, Visibility.Collapsed)
                };
            }

            if (value is string str)
            {
                return str.Length > 0
                    ? notNullVisibility
                    : nullVisibility;
            }

            return (value is not null)
                    ? notNullVisibility
                    : nullVisibility;
        }
        catch
        {
            return notNullVisibility;
        }
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        string language
    )
        => throw new NotImplementedException();
}

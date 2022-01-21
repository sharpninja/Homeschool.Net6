//using Homeschool.App.Data;

namespace Homeschool.App.Common;

using Data;

using System;

using Microsoft.UI.Xaml.Media.Imaging;

public class ImageLoader
{
    public static string Get_source(DependencyObject obj)
        => (string)obj.GetValue(ImageLoader._sourceProperty);

    public static void Set_source(DependencyObject obj, string value)
    {
        obj.SetValue(ImageLoader._sourceProperty, value);
    }

    // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty _sourceProperty =
        DependencyProperty.RegisterAttached("_source", typeof(string), typeof(ImageLoader), new PropertyMetadata(string.Empty, ImageLoader.OnPropertyChanged));

    private static async void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Image image)
        {
            ControlInfoDataItem item = await ControlInfoDataSource.Instance.GetItemAsync(e.NewValue != null
                ? e.NewValue.ToString()
                : null).ConfigureAwait(false);

            if (item == null || item.ImagePath == null)
            {
                return;
            }

            Uri imageUri = new(item.ImagePath, UriKind.Absolute);
            BitmapImage imageBitmap = new(imageUri);
            image.Source = imageBitmap;
        }
    }
}
namespace Homeschool.App.Helper;

using System.Collections.Generic;

using Windows.Storage;

using Microsoft.UI.Xaml.Media;

public static class UiHelper
{
    public static bool IsScreenshotMode { get; set; }
    public static StorageFolder ScreenshotStorageFolder { get; set; } = ApplicationData.Current.LocalFolder;

    public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
        => start.GetDescendants().OfType<T>();

    public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
    {
        var queue = new Queue<DependencyObject>();
        int count1 = VisualTreeHelper.GetChildrenCount(start);

        for (int i = 0; i < count1; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(start, i);
            yield return child;
            queue.Enqueue(child);
        }

        while (queue.Count > 0)
        {
            DependencyObject parent = queue.Dequeue();
            int count2 = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count2; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
                queue.Enqueue(child);
            }
        }
    }
}
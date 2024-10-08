namespace Homeschool.App.Helper;

using System.Collections.Generic;

using Microsoft.UI.Xaml.Media;

public static class UIHelper
{

    public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
        => start.GetDescendants().OfType<T>();

    public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
    {
        var queue = new Queue<DependencyObject>();
        var count1 = VisualTreeHelper.GetChildrenCount(start);

        for (var i = 0; i < count1; i++)
        {
            var child = VisualTreeHelper.GetChild(start, i);
            yield return child;
            queue.Enqueue(child);
        }

        while (queue.Count > 0)
        {
            var parent = queue.Dequeue();
            var count2 = VisualTreeHelper.GetChildrenCount(parent);

            for (var i = 0; i < count2; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;

                queue.Enqueue(child);
            }
        }
    }
}
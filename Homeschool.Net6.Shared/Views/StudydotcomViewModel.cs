namespace Homeschool.App.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DomainModels.Courses;

using Proxy;

using System.Collections.Immutable;
using System.Diagnostics;

[ObservableObject]
public partial class StudydotcomViewModel
{
    [ObservableProperty]
    protected List<LessonQueueItem> _lessonQueue = new();

    [ObservableProperty]
    protected List<LessonQueueItem> _completedLessons = new();

    [ObservableProperty]
    protected List<LessonQueueItem> _incompleteLessons = new();

    [ObservableProperty]
    protected LessonQueueItem? _nextLesson = new()
    {
        MarkedCompleteDateTime = DateTimeOffset.MinValue
    };

    [ICommand]
    public void MarkCompleted()
    {
        if (NextLesson is null)
        {
            return;
        }

        MainViewModel.SetStatus($"Marked {NextLesson.LessonTitle} as Completed.");

        NextLesson.MarkedCompleteDateTime = DateTimeOffset.Now;

        Proxy service = App.Services!.GetRequiredService<Proxy>();
        var result = service.MarkLessonCompleted(
            NextLesson.LessonUid,
            NextLesson.MarkedCompleteDateTime.Value
        ).GetAwaiter().GetResult();

        //if (Debugger.IsAttached)
        //{
        //    Debugger.Break();
        //}

        if (result is null)
        {
            NextLesson.MarkedCompleteDateTime = DateTime.MinValue;
            MainViewModel.SetStatus($"Reverted marking completed due to server returning null.");
            return;
        }

        _completedLessons.Add(NextLesson);
        _incompleteLessons.Remove(NextLesson);
        LessonQueue = _incompleteLessons
            .Union(_completedLessons)
            .OrderBy(i =>
                (i.MarkedCompleteDateTime is null ||
                 i.MarkedCompleteDateTime == DateTimeOffset.MinValue)
                    ? 0 : 1
            )
            .ThenBy(i => i.Index)
            .ToList();

        SetNextLesson();

    }

    public async Task<bool> LoadLessonQueueAsync()
    {
        Proxy service = App.Services!.GetRequiredService<Proxy>();
        LessonQueueItem[]? results = await service.GetLessonQueueAsync();

        if (results is null
            or
            {
                Length: 0
            })
        {
            return false;
        }

        _completedLessons.Clear();
        LessonQueue = _incompleteLessons =
            results
                .OrderBy(
                    i => (i.MarkedCompleteDateTime is null ||
                          i.MarkedCompleteDateTime == DateTimeOffset.MinValue)
                    ? 0
                    : 1
                )
                .ThenBy(i => i.Index)
                .ToList();

        SetNextLesson();

        MainViewModel.SetStatus($"Loaded {LessonQueue.Count} Queued Lessons.");

        return true;
    }

    private void SetNextLesson()
    {
        NextLesson = LessonQueue.FirstOrDefault(l => l.MarkedCompleteDateTime is null);
    }
}

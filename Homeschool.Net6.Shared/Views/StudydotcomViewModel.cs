﻿namespace Homeschool.App.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DomainModels.Courses;

using Proxy;

using System.Reflection;

[ObservableObject]
public partial class StudydotcomViewModel
{
    public ILogger<StudydotcomViewModel> Logger
    {
        get;
    }

    public WcfProxy WcfProxy
    {
        get;
    }

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

    public StudydotcomViewModel(ILogger<StudydotcomViewModel> logger, WcfProxy wcfProxy)
    {
        Logger = logger;
        WcfProxy = wcfProxy;
    }

    [ICommand]
    public void MarkCompleted()
    {
        if (NextLesson is null)
        {
            return;
        }

        MainViewModel.SetStatus($"Marked {NextLesson.LessonTitle} as Completed.");

        NextLesson.MarkedCompleteDateTime = DateTimeOffset.Now;


        var result = WcfProxy.MarkLessonCompleted(
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
        //if (Debugger.IsAttached)
        //{
        //    Debugger.Break();
        //}
        Task<LessonQueueItem[]?> GetLessonQueueAsync()
        {
            try
            {
                Task<LessonQueueItem[]?> result = WcfProxy.GetLessonQueueAsync();

                return result;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        try
        {
            LessonQueueItem[]? results = await GetLessonQueueAsync();

            if (results is null
                or
                {
                    Length: 0
                })
            {
                return false;
            }

            _completedLessons.Clear();
            LessonQueue = _incompleteLessons = results.OrderBy(
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
        catch (Exception ex)
        {
            Logger.LogError(ex, MethodBase.GetCurrentMethod().Name);
            MainViewModel.SetStatus(ex.Message);
        }

        return false;
    }

    private void SetNextLesson()
    {
        NextLesson = LessonQueue.FirstOrDefault(l => l.MarkedCompleteDateTime is null);
    }

    public bool CanMarkCompleted => NextLesson?.CanComplete ?? false;
}

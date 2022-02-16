namespace Homeschool.DomainModels.Courses;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Data;

[ObservableObject]
public partial class LessonQueueItem
{
    public LessonQueueItem() : this(new HsLessonQueue(), 0)
    {
    }

    public LessonQueueItem(HsLessonQueue item, int index)
    {
        CourseUid = item.course_uid;
        ChapterUid = item.chapter_uid;
        LessonUid = item.lesson_uid;
        LessonIsPastDue = item.lesson_is_past_due;
        CourseTitle = item.course_title;
        ChapterTitle = item.chapter_title;
        LessonTitle = item.lesson_title;
        LessonUrl = item.lesson_url;
        CourseIcon = $"/Assets/{item.course_icon}";
        MarkedCompleteDateTime = item.lesson_marked_completed;
        Index = index;
    }

    [ ObservableProperty ]
    protected int index;

    [ ObservableProperty ]
    protected string courseIcon;

    [ ObservableProperty ]
    protected Guid courseUid;

    [ ObservableProperty ]
    protected Guid chapterUid;

    [ ObservableProperty ]
    protected Guid lessonUid;

    [ ObservableProperty ]
    protected bool lessonIsPastDue;

    [ ObservableProperty ]
    protected string courseTitle;

    [ ObservableProperty ]
    protected string chapterTitle;

    [ ObservableProperty ]
    protected string lessonTitle;

    [ ObservableProperty ]
    protected string lessonUrl;

    [ ObservableProperty, AlsoNotifyChangeFor(nameof(MarkedCompleteDateTimeFormatted)),
    AlsoNotifyChangeFor(nameof(Visibility))]
    protected DateTimeOffset? markedCompleteDateTime;

    public string MarkedCompleteDateTimeFormatted
        => $"{markedCompleteDateTime?.LocalDateTime.ToShortTimeString()}";

    public bool Visibility => courseUid != Guid.Empty &&
                              (markedCompleteDateTime is null ||
                              markedCompleteDateTime == DateTimeOffset.MinValue);
}

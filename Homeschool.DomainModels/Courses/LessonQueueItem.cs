namespace Homeschool.DomainModels.Courses;

using Data;

public record LessonQueueItem
{
    public LessonQueueItem() : this(new HsLessonQueue())
    {
    }

    public LessonQueueItem(HsLessonQueue item)
    {
        CourseUid = item.course_uid;
        ChapterUid = item.chapter_uid;
        LessonUid = item.lesson_uid;
        LessonIsPastDue = item.lesson_is_past_due;
        CourseTitle = item.course_title;
        ChapterTitle = item.chapter_title;
        LessonTitle = item.lesson_title;
        LessonUrl = item.lesson_url;
    }

    public Guid CourseUid { get; init; }
    public Guid ChapterUid { get; init; }
    public Guid LessonUid { get; init; }
    public bool LessonIsPastDue { get; init; }
    public string CourseTitle { get; init; }
    public string ChapterTitle { get; init; }
    public string LessonTitle { get; init; }
    public string LessonUrl { get; init; }
}

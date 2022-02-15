namespace Homeschool.Data;

public partial class HsLessonQueue
{
    public HsLessonQueue()
    {
        course_uid = Guid.Empty;
        chapter_uid = Guid.Empty;
        lesson_uid = Guid.Empty;
        lesson_is_past_due = false;
        course_title = string.Empty;
        chapter_title = string.Empty;
        lesson_title = string.Empty;
        lesson_url = string.Empty;
    }
}

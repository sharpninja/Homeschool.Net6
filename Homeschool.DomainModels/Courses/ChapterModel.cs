namespace Homeschool.DomainModels.Courses;

using System.Collections.Immutable;
using System.Runtime.Serialization;

using Data;

using Grades;

[DataContract]
public class ChapterModel : IGradeContainer
{
    public ChapterModel()
    {
        Uid = Guid.NewGuid();
    }

    public ChapterModel(CourseModel? course, HsChapter entity)
    {
        Uid = entity.ChapUid;
        ChapterTitle = entity.ChapTitle;
        Course = course;
        Lessons = entity.HsLessons.Select(hsl => new LessonModel(this, hsl))
            .ToImmutableList();
    }

    [DataMember]
    public Guid Uid { get; init; }

    [ DataMember ]
    public ImmutableList<AssessmentGrade>? Grades
    {
        get;
        init;
    }

    [ DataMember ]
    public string? ChapterTitle { get; init; }

    [ DataMember ]
    public string? ChapterShortTitle { get; init; }

    [ DataMember ]
    public Uri? ChapterFullUri { get; init; }

    public CourseModel? Course { get; init; }

    [ DataMember ]
    public ImmutableList<LessonModel>? Lessons { get; init; }
}

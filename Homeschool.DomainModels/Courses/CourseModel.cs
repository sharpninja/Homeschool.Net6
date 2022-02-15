namespace Homeschool.DomainModels.Courses;

using System.Collections.Immutable;
using System.Runtime.Serialization;

using Data;

using Homeschool.DomainModels.Grades;

[ DataContract ]
public class CourseModel : IGradeContainer
{
    public CourseModel()
    {
        Uid = Guid.NewGuid();
        Chapters = Array.Empty<ChapterModel>()
            .ToImmutableList();
    }

    public CourseModel(HsCourse entity)
    {
        Uid = entity.CourUid;
        CourseTitle = entity.CourTitle;
        Chapters = entity.HsChapters.Select(hsc => new ChapterModel(this, hsc))
            .ToImmutableList();
    }

    [DataMember ]
    public Guid Uid { get; init; }

    [ DataMember ]
    public ImmutableList<AssessmentGrade>? Grades
    {
        get;
        init;
    }

    [ DataMember ]
    public string? CourseTitle { get; init; }

    [ DataMember ]
    public string? CourseShortTitle { get; init; }

    [ DataMember ]
    public Uri? CourseFullUri { get; init; }

    [ DataMember ]
    public ImmutableList<ChapterModel> Chapters { get; init; }
}
namespace Homeschool.DomainModels.Courses;

using System.Collections.Immutable;
using System.Runtime.Serialization;

using Data;

using Grades;

[ DataContract ]
public class LessonModel : IGradeContainer
{
    private const string BASE_URL = "https://www.study.com";
    public LessonModel()
        => Uid = Guid.NewGuid();

    public LessonModel(ChapterModel? chapter, HsLesson entity)
    {
        Uid = entity.LessUid;
        LessonTitle = entity.LessTitle;
        Chapter = chapter;
        LessonFullUri = new Uri(new Uri(LessonModel.BASE_URL), entity.LessUrl);
        Grades = entity.GetGrades()
            .Select(g => new AssessmentGrade(GradesScopes.Lesson, Uid, g))
            .ToImmutableList();
    }

    [DataMember ]
    public Guid Uid { get; init; }

    [ DataMember ]
    public string? LessonTitle { get; init; }

    [ DataMember ]
    public string? LessonShortTitle { get; init; }

    [ DataMember ]
    public Uri? LessonFullUri { get; init; }

    [ DataMember ]
    public ChapterModel? Chapter { get; init; }

    [ DataMember ]
    public ImmutableList<AssessmentGrade>? Grades { get; init; }
}

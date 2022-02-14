namespace Homeschool.DomainModels.Grades;

using System.Collections.Immutable;

public interface IGradeContainer
{
    Guid Uid { get; }

    ImmutableList<AssessmentGrade>? Grades { get; }
}

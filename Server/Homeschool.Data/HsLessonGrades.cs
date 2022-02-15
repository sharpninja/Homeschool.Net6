namespace Homeschool.Data;

using Context;

public partial class HsLesson
{
    public List<HsGradebook> GetGrades()
        => HomeschoolContext.GetNewContext()
            .HsGradebooks.Where(g => g.GradLessTitle == LessSlug)
            .ToList();
}

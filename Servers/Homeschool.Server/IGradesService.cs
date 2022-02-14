namespace Homeschool.Server
{
    [ServiceContract]
    public interface IGradesService
    {
        [OperationContract]
        AssessmentGrade[]? GetGradesByParent(Guid parent, GradesScopes scope);

        [OperationContract]
        AssessmentGrade[]? GetGradesByFilter(GradesFilter filter);
    }
}

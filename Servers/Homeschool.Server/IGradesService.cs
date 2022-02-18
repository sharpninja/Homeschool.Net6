namespace Homeschool.Server
{
    using DomainModels.Courses;

    [ServiceContract]
    public interface IGradesService
    {
        [OperationContract]
        AssessmentGrade[]? GetGradesByParent(Guid parent, GradesScopes scope);

        [OperationContract]
        AssessmentGrade[]? GetGradesByFilter(GradesFilter filter);

        [ OperationContract ]
        LessonQueueItem[]? GetLessonQueue(int? min, int? max);

        [ OperationContract ]
        LessonModel? MarkLessonCompleted(Guid lessonUid, DateTimeOffset timestamp);

        [ OperationContract ]
        LessonModel? MarkLessonOpened(Guid lessonUid, DateTimeOffset timestamp);
    }
}

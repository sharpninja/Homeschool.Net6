namespace Homeschool.DomainModels.Grades;

using System.Runtime.Serialization;

using Data;

[ DataContract ]
public class AssessmentGrade
{
    public AssessmentGrade()
    {

    }

    public AssessmentGrade(GradesScopes scope, Guid parentUid, HsGradebook entity)
    {
        Scope = scope;
        ParentUid = parentUid;
        DatePassed = entity.GradDateCompleted;
        TotalPoints = entity.GradTotalPoints;
        EarnedPoints = entity.GradPointsEarned;
    }

    [ DataMember ]
    public GradesScopes Scope { get; init; }

    [DataMember ]
    public Guid ParentUid { get; init; }

    [ DataMember ]
    public DateTime? DatePassed { get; init; }

    [ DataMember ]
    public int? TotalPoints { get; init; }

    [ DataMember ]
    public int? EarnedPoints { get; set; }

    [IgnoreDataMember ]
    public string? Percentage => EarnedPoints is not null && TotalPoints is not null && TotalPoints > 0
        ? $"{(decimal)EarnedPoints / (decimal)TotalPoints:P2}"
        : null;

    [ IgnoreDataMember ]
    public Grade Grade => new(this);

    public override string ToString()
    {
        return this.DatePassed is not null && TotalPoints > 0
            ? ((Func<string>)(()=>$"Scope: {Scope}, Parent: {ParentUid}, Passed On: {DatePassed}, Earned: {EarnedPoints}, Total: {TotalPoints}, Percentage: {Percentage}, Grade: {Grade}"))()
            : ((Func<string>)(() => $"Scope: {Scope}, Parent: {ParentUid}, Not completed."))();
    }
}
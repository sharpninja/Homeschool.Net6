namespace Homeschool.DomainModels.Grades;

using System.Runtime.Serialization;

[DataContract]
public class GradesFilter
{
    [ DataMember ]
    public GradesScopes Scope { get; set; } = GradesScopes.All;

    [ DataMember ]
    public Guid[] Parents { get; set; } = Array.Empty<Guid>();

    [ DataMember ]
    public Range GradeRange { get; set; } = new (0, 5);

    [ DataMember ]
    public GenericRange<DateOnly> DateRange { get; set; } = new (
        new DateOnly(2021, 11, 1),
        DateOnly.FromDateTime(DateTime.Today)
    );
}

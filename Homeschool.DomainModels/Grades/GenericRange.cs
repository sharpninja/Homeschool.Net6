namespace Homeschool.DomainModels.Grades;

using System.Runtime.Serialization;

[ DataContract ]
public class GenericRange<TType>
    where TType : IComparable<TType>
{
    [ DataMember ]
    public TType From
    {
        get;
        set;
    }

    [ DataMember ]
    public TType To
    {
        get;
        set;
    }

    [ IgnoreDataMember ]
    public bool IsAscending => From.CompareTo(To) > 0;

    public GenericRange(TType from, TType to)
    {
        From = @from;
        To = to;
    }
}

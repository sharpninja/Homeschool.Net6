namespace Homeschool.Proxy;

using System;
using System.ServiceModel;

internal static class ClientLogic
{
    static ClientLogic()
    {
        Settings = BuildClientSettings();
        Log = null;
    }

    private static Settings Settings { get; }
    private static Action<string>? Log { get; set; }

    public static void SetLog(Action<string>? log)
        => Log = log;

    public static Settings BuildClientSettings()
    {
        const string HOSTNAME = "LOCALHOST";

        var hostname = HOSTNAME;

        var settings = new Settings().SetDefaults(
            hostname, "GradesService");

        return settings;
    }

    public static AssessmentGrade[]? GetGradesByParent(Guid parent, GradesScopes scope)
    {
        var getGradesByParent = (Func<Homeschool.Server.IGradesService, AssessmentGrade[]?>)(channel
            => channel.GetGradesByParent(
                parent,
                scope
            ));

        AssessmentGrade[]? result = Array.Empty<AssessmentGrade>();
        Log?.Invoke(
            $"BasicHttp: GetGradesByParent(parent: {parent}, scope: {scope}) => " +
            (result = getGradesByParent.WcfInvoke(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                Settings.BasicHttpAddress
            ))
        );

        return result;
    }

    public static AssessmentGrade[]? GetGradesByFilter(GradesFilter filter)
    {
        var getGradesByFilter = (Func<Homeschool.Server.IGradesService, AssessmentGrade[]?>)(channel
            => channel.GetGradesByFilter(filter));

        AssessmentGrade[]? result = Array.Empty<AssessmentGrade>();
        Log?.Invoke(
            $"BasicHttp: GetGradesByFilter(filter: {filter}) => " +
            (result = getGradesByFilter.WcfInvoke(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                Settings.BasicHttpAddress
            ))
        );

        return result;

    }
}

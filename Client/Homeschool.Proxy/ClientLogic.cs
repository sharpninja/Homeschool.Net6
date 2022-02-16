namespace Homeschool.Proxy;

using System;
using System.ServiceModel;

using DomainModels.Courses;

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

    public static Settings BuildClientSettings(string? hostname = null)
    {
        const string HOSTNAME = "LOCALHOST";

        Settings settings = new Settings().SetDefaults(
            hostname ?? HOSTNAME, "GradesService");

        return settings;
    }

    public static async Task<AssessmentGrade[]?> GetGradesByParentAsync(Guid parent, GradesScopes scope)
    {
        var getGradesByParent =
            (Func<Homeschool.Server.IGradesService, AssessmentGrade[]?>)(
                channel => channel.GetGradesByParent(parent, scope));

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

        AssessmentGrade[]? result =
            await getGradesByParent.WcfInvokeAsync(binding, Settings.BasicHttpAddress!);

        Log?.Invoke($"BasicHttp: GetGradesByParentAsync(parent: {parent}, scope: {scope}) => {result?.Length}");

        return result;
    }

    public static async Task<LessonQueueItem[]> GetLessonQueueAsync(int? min = 7, int? max = 7)
    {
        var getLessonQueue
            = (Func<Homeschool.Server.IGradesService, LessonQueueItem[]?>)(channel
                => channel.GetLessonQueue(min, max));

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

        LessonQueueItem[]? result
            = await getLessonQueue.WcfInvokeAsync(binding, Settings.BasicHttpAddress!);

        Log?.Invoke($"BasicHttp: GetLessonQueueAsync(min: {min}, max: {max}) => {result?.Length}");

        return result ?? Array.Empty<LessonQueueItem>();
    }

    public static Task<LessonModel?> MarkLessonCompleted(Guid lessonUid, DateTimeOffset timestamp)
    {
        var markLessonComplete
            = (Func<Homeschool.Server.IGradesService, LessonModel?>)(channel
                => channel.MarkLessonCompleted(lessonUid, timestamp));

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

        var result
            = markLessonComplete.WcfInvokeAsync(binding, Settings.BasicHttpAddress!);

        Log?.Invoke($"BasicHttp: MarkLessonCompleted(lessonUid: {lessonUid}, timestamp: {timestamp})");

        return result;
    }


    public static async Task<AssessmentGrade[]?> GetGradesByFilter(GradesFilter filter)
    {
        var getGradesByFilter =
            (Func<Homeschool.Server.IGradesService, AssessmentGrade[]?>)(
                channel => channel.GetGradesByFilter(filter));

        AssessmentGrade[]? result = await getGradesByFilter.WcfInvokeAsync(
            new BasicHttpBinding(BasicHttpSecurityMode.None),
            Settings.BasicHttpAddress!
        );

        Log?.Invoke($"BasicHttp: GetGradesByFilter(filter: {filter}) => {result?.Length}");

        return result;

    }
}

namespace Homeschool.Proxy;

using System;
using System.ServiceModel;

using DomainModels.Courses;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public  class HomeschoolClientLogic
{
    public ILogger<HomeschoolClientLogic> Logger
    {
        get;
    }

    public HomeschoolClientLogic(IConfiguration config, ILogger<HomeschoolClientLogic> logger)
    {
        Logger = logger;
        var hostName = config["ServiceHost"];

        Logger.LogInformation($"hostName: {hostName}");

        Settings = BuildClientSettings(hostName);
        Log = null;
    }

    private  Settings Settings { get; }
    private  Action<string>? Log { get; set; }

    public  void SetLog(Action<string>? log)
        => Log = log;

    public  Settings BuildClientSettings(string? hostname = null)
    {
        const string HOSTNAME = "LOCALHOST";

        Settings settings = new Settings().SetDefaults(
            hostname ?? HOSTNAME, "GradesService");

        return settings;
    }

    public  async Task<AssessmentGrade[]?> GetGradesByParentAsync(Guid parent, GradesScopes scope)
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

    public  async Task<LessonQueueItem[]> GetLessonQueueAsync(int? min = 7, int? max = 7)
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

    public  Task<LessonModel?> MarkLessonCompleted(Guid lessonUid, DateTimeOffset timestamp)
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


    public  async Task<AssessmentGrade[]?> GetGradesByFilter(GradesFilter filter)
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

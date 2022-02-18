namespace Homeschool.Proxy;

using Server;

using System.Reflection;

public class HomeschoolClientLogic
{
    public ILogger<HomeschoolClientLogic> Logger
    {
        get;
    }

    public HomeschoolClientLogic(WcfSettings settings,
        ILogger<HomeschoolClientLogic> logger)
    {
        Logger = logger;

        WcfSettings = settings;
        settings.Initialize("GradesService");

        Log = LogMethod;
    }

    private void LogMethod(string value)
        => Logger.LogInformation(value);

    private WcfSettings WcfSettings { get; }

    public Action<string>? Log { get; private set; }

    public async Task<AssessmentGrade[]?> GetGradesByParentAsync(Guid parent, GradesScopes scope)
    {
        var getGradesByParent =
            (Func<Homeschool.Server.IGradesService, AssessmentGrade[]?>)(
                channel => channel.GetGradesByParent(parent, scope));

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

        AssessmentGrade[]? result =
            await getGradesByParent.WcfInvokeAsync(binding, WcfSettings.BasicHttpAddress!);

        Log?.Invoke($"BasicHttp: GetGradesByParentAsync(parent: {parent}, scope: {scope}) => {result?.Length}");

        return result;
    }

    protected TResult SafelyExecuteAsync<TResult>(Func<TResult> toExecute)
        where TResult : Task
    {
        try
        {
            var result = toExecute();

            if (result is Task task)
            {
                if (task.IsFaulted)
                {
                    throw new ApplicationException(
                        "SafelyExecuteAsync has received a faulted task as a result.",
                        task.Exception
                    );
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, MethodBase.GetCurrentMethod()?.Name ?? "Unknown");

            throw;
        }
    }

    private TResult? getLessonQueue<TContract, TResult>(TContract channel, params string[] parameters)
        where TResult : class
    {
        // ReSharper disable once ArrangeMethodOrOperatorBody
        return (channel as IGradesService)?.GetLessonQueue(
            int.Parse(parameters[0]),
            int.Parse(parameters[1])
        ) as TResult;
    }

    public Task<LessonQueueItem[]> GetLessonQueueAsync(int? min = 6, int? max = 6)
    {
        async Task<LessonQueueItem[]> ToExecute()
        {
            BasicHttpBinding binding = new (BasicHttpSecurityMode.None);

            var del =
                new WcfDelegate<IGradesService, LessonQueueItem[]?>(
                    getLessonQueue<IGradesService, LessonQueueItem[]>);

            LessonQueueItem[]? result = await del.WcfInvokeAsync(
                binding,
                WcfSettings.BasicHttpAddress!,
                parms: new [] { $"{min ?? 6}", $"{max ?? 6}", }
            );

            Logger.LogInformation($"BasicHttp: GetLessonQueueAsync(min: {min}, max: {max}) => {result?.Length}");

            LessonQueueItem[] taskResult = result ?? Array.Empty<LessonQueueItem>();

            return taskResult;
        }

        return SafelyExecuteAsync(ToExecute);
    }

    public Task<LessonModel?> MarkLessonCompleted(Guid lessonUid, DateTimeOffset timestamp)
    {
        var markLessonComplete
            = (Func<IGradesService, LessonModel?>)(channel
                => channel.MarkLessonCompleted(lessonUid, timestamp));

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

        var result
            = markLessonComplete.WcfInvokeAsync(binding, WcfSettings.BasicHttpAddress!);

        Log?.Invoke($"BasicHttp: MarkLessonCompleted(lessonUid: {lessonUid}, timestamp: {timestamp})");

        return result;
    }


    public async Task<AssessmentGrade[]?> GetGradesByFilter(GradesFilter filter)
    {
        var getGradesByFilter =
            (Func<Homeschool.Server.IGradesService, AssessmentGrade[]?>)(
                channel => channel.GetGradesByFilter(filter));

        AssessmentGrade[]? result = await getGradesByFilter.WcfInvokeAsync(
            new BasicHttpBinding(BasicHttpSecurityMode.None),
            WcfSettings.BasicHttpAddress!
        );

        Log?.Invoke($"BasicHttp: GetGradesByFilter(filter: {filter}) => {result?.Length}");

        return result;

    }
}

namespace Homeschool.Server;

using Data.Context;

using DomainModels.Courses;
using DomainModels.Grades;

using Microsoft.EntityFrameworkCore;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class GradesService : IGradesService, IHostedService
{
    public HomeschoolContext Context
    {
        get;
    }

    public ILogger<GradesService> Logger
    {
        get;
    }

    public GradesService(ILogger<GradesService> logger, HomeschoolContext context)
    {
        Logger = logger;
        Context = context;
    }

    public AssessmentGrade[]? GetGradesByParent(Guid parent, GradesScopes scope)
    {
        Logger.LogInformation($"GetGradesByParent: parent: {parent}, scope: {scope}");
        Logger.LogInformation($"GetGradesByParent: ctx.HsCourses.Count(): {Context.HsCourses.Count()}");
        Logger.LogInformation($"GetGradesByParent: ctx.HsChapters.Count(): {Context.HsChapters.Count()}");
        Logger.LogInformation($"GetGradesByParent: ctx.HsLessons.Count(): {Context.HsLessons.Count()}");

        switch (scope)
        {
            case GradesScopes.Lesson:
                var parentLesson = Context.HsLessons.FirstOrDefault(l => l.LessUid == parent);

                if (parentLesson is null)
                {
                    Logger.LogWarning($"GetGradesByParent: Failed to locate {parent} in Lessons.");
                    return null;
                }

                var lesson = new LessonModel(null, parentLesson);

                return lesson.Grades.ToArray();

            case GradesScopes.Chapter:
                var parentChapter = Context.HsChapters.Include("HsLessons").FirstOrDefault(l => l.ChapUid == parent);

                if (parentChapter is null)
                {
                    Logger.LogWarning($"GetGradesByParent: Failed to locate {parent} in Chapters.");
                    return null;
                }

                var chapter = new ChapterModel(null, parentChapter);

                return chapter.Lessons.SelectMany(l => l.Grades)
                    .ToArray();

            case GradesScopes.Course:
                var parentCourse = Context.HsCourses.Include("HsChapters")
                    .Include("HsChapters.HsLessons")
                    .FirstOrDefault(l => l.CourUid == parent);

                if (parentCourse is null)
                {
                    Logger.LogWarning($"GetGradesByParent: Failed to locate {parent} in Courses.");
                    return null;
                }

                var course = new CourseModel(parentCourse);

                return course.Chapters.SelectMany(c => c.Lessons.SelectMany(l => l.Grades))
                    .ToArray();

            case GradesScopes.All:
                return GetGradesByParent(parent, GradesScopes.Course) ??
                       GetGradesByParent(parent, GradesScopes.Chapter) ??
                       GetGradesByParent(parent, GradesScopes.Lesson);
        }

        return Array.Empty<AssessmentGrade>();
    }

    public AssessmentGrade[] GetGradesByFilter(GradesFilter filter)
        => throw new NotImplementedException();

    public LessonQueueItem[]? GetLessonQueue(int? min, int? max)
    {
        var queueItems =
            Context.Procedures.usp_GetLessonQueueAsync(min, max)
                .GetAwaiter()
                .GetResult();

        return queueItems.Select((q, index) => new LessonQueueItem(q, index))
            .ToArray();
    }

    public LessonModel? MarkLessonCompleted(Guid lessonUid, DateTimeOffset timestamp)
    {
        var hsLesson = Context
            .HsLessons
            .FirstOrDefault(l => l.LessUid == lessonUid);

        if (hsLesson is null)
        {
            return null;
        }

        hsLesson.LessMarkedCompleted = timestamp.LocalDateTime;

        Context.SaveChanges();

        var result = new LessonModel(null, hsLesson);

        return result;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly GradesService _gradesService;
    private readonly ILogger<WindowsBackgroundService> _logger;

    public WindowsBackgroundService(
        GradesService gradesService,
        ILogger<WindowsBackgroundService> logger
    )
        => (_gradesService, _logger) = (gradesService, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(
            static (context, options) =>
            {
                options.AllowSynchronousIO = true;
            }
        );
        builder.Services.AddSingleton(
                provider =>
                {
                    string cs = provider.GetRequiredService<IConfiguration>()["ConnectionString"];

                    return new DbContextOptionsBuilder<HomeschoolContext>()
                        .EnableDetailedErrors(true)
                        .UseSqlServer(
                            cs,
                            builder =>
                            {
                            }
                        )
                        .Options;
                }
            )
            .AddTransient<GradesService>()
            .AddDbContext<HomeschoolContext>()
            .AddServiceModelServices();

        var app = builder.Build();

        app.UseServiceModel(
            builder =>
            {
                builder.AddService<GradesService>()
                    .AddServiceEndpoint<GradesService, IGradesService>(
                        new BasicHttpBinding(),
                        "/GradesService/basichttp"
                    );
            }
        );

        app.Run();
    }
}
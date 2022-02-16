namespace Homeschool.Proxy
{
    using Data;

    using DomainModels.Courses;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Proxy
    {
        public void Initialize(IServiceProvider services)
        {
            this.Config = services.GetRequiredService<IConfiguration>()!;

            this.Settings = ClientLogic.BuildClientSettings(Config["ServiceHost"])!;

            this.Logger = services.GetService<ILogger<Proxy>>()!;
        }

        public async Task<string?> Test()
        {
            void Log(string value) => Logger.LogInformation(value);

            ClientLogic.SetLog(Log);
            //var result = ClientLogic.GetGradesByParentAsync(
            //    Guid.Parse("99F38BA7-2F27-41BE-85C2-BA2323B273B8"),
            //    GradesScopes.All);

            var result = await GetLessonQueueAsync();

            Log($"lesson: [{result}]");

            if (result is null)
            {
                return "Null results;";
            }

            Log($"enumerable.count(): {result.Count()}");
                return string.Join("\n", result.Select(r => r.ToString()));
        }

        public async Task<LessonQueueItem[]> GetLessonQueueAsync(int? min = 7, int? max = 7)
        {
            void Log(string? value)
            {
                string msg = value ?? "<<null>>";
                Logger.LogInformation(msg);
            }

            ClientLogic.SetLog(Log);
            var result = await ClientLogic.GetLessonQueueAsync(min, max);

            Log($"lesson: [{result}]");

            return result;
        }

        public Task<LessonModel?> MarkLessonCompleted(
            Guid lessonUid,
            DateTimeOffset timestamp
        )
        {
            void Log(string? value)
            {
                string msg = value ?? "<<null>>";
                Logger.LogInformation(msg);
            }

            ClientLogic.SetLog(Log);
            var result = ClientLogic.MarkLessonCompleted(lessonUid, timestamp);

            Log($"lesson: [{result}]");

            return result;
        }

        public ILogger<Proxy> Logger
        {
            get;
            set;
        } = null!;

        public IConfiguration Config
        {
            get;
            set;
        } = null!;

        public Settings Settings
        {
            get;
            set;
        } = null!;
    }
}
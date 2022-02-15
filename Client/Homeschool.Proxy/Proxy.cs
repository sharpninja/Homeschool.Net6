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

            this.Settings = ClientLogic.BuildClientSettings()!;

            this.Logger = services.GetService<ILogger<Proxy>>()!;
        }

        public string? Test()
        {
            void Log(string value) => Logger.LogInformation(value);

            ClientLogic.SetLog(Log);
            //var result = ClientLogic.GetGradesByParent(
            //    Guid.Parse("99F38BA7-2F27-41BE-85C2-BA2323B273B8"),
            //    GradesScopes.All);

            var result = GetLessonQueue();

            Log($"lesson: [{result}]");

            if (result is null)
            {
                return "Null results;";
            }

            Log($"enumerable.count(): {result.Count()}");
                return string.Join("\n", result.Select(r => r.ToString()));
        }

        public LessonQueueItem[] GetLessonQueue(int? min = 7, int? max = 7)
        {
            void Log(string value)
                => Logger.LogInformation(value);

            ClientLogic.SetLog(Log);
            var result = ClientLogic.GetLessonQueue(min, max);

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
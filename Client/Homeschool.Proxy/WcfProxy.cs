namespace Homeschool.Proxy
{
    public class WcfProxy
    {
        private HomeschoolClientLogic HomeschoolClientLogic { get; set; }

        public WcfProxy(ILogger<WcfProxy> logger, HomeschoolClientLogic logic)
        {
            HomeschoolClientLogic = logic;

            Logger = logger;
        }

        public async Task<string?> Test()
        {
            var result = await GetLessonQueueAsync();

            Logger.LogInformation($"lesson: [{result}]");

            if (result is null)
            {
                return "Null results;";
            }

            Logger.LogInformation($"enumerable.count(): {result.Count()}");
                return string.Join("\n", result.Select(r => r.ToString()));
        }

        public Task<LessonQueueItem[]> GetLessonQueueAsync(int? min = 7, int? max = 7)
        {
            var result = HomeschoolClientLogic.GetLessonQueueAsync(min, max);

            Logger.LogInformation($"lesson: [{result}]");

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

            var result = HomeschoolClientLogic.MarkLessonCompleted(lessonUid, timestamp);

            Logger.LogInformation($"lesson: [{result}]");

            return result;
        }

        public ILogger<WcfProxy> Logger
        {
            get;
            set;
        } = null!;
    }
}
namespace Homeschool.Proxy
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Proxy
    {
        public void Initialize(IServiceProvider services)
        {
            this.Config = services.GetRequiredService<IConfiguration>();

            this.Settings = ClientLogic.BuildClientSettings();

            this.Logger = services.GetService<ILogger<Proxy>>()!;
        }

        public string? Test()
        {
            void Log(string value) => Logger.LogInformation(value);

            ClientLogic.SetLog(Log);
            var result = ClientLogic.GetGradesByParent(
                Guid.Parse("99F38BA7-2F27-41BE-85C2-BA2323B273B8"),
                GradesScopes.All);

            Log($"lesson: [{result}]");

            if (result is IEnumerable<AssessmentGrade> enumerable)
            {
                Log($"enumerable.count(): {enumerable.Count()}");
                return string.Join("\n", enumerable);
            }
            else
            {
                return result?.ToString();
            }
        }

        public ILogger<Proxy> Logger
        {
            get;
            set;
        }

        public IConfiguration Config
        {
            get;
            set;
        }

        public Settings Settings
        {
            get;
            set;
        }
    }
}
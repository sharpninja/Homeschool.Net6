using System;

using Xunit;
using Xunit.Abstractions;

namespace Homeschool.Aws.Client.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class LambdasTests
    {
        public ITestOutputHelper OutputHelper
        {
            get;
        }

        private IHost AppHost { get; }

        public LambdasTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;

            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureAppConfiguration(
                builder =>
                {
                    builder.AddUserSecrets(typeof(Lambdas).Assembly);
                }
            );

            builder.ConfigureServices(
                (context, collection) =>
                {
                    collection.AddLogging(loggingBuilder => loggingBuilder.AddXunit(outputHelper));
                    collection.AddTransient<Lambdas>();
                }
            );

            AppHost = builder.Build();

        }

        [Fact()]
        public async Task GetQueueTest()
        {
            var logging = AppHost.Services.GetRequiredService<ILogger<LambdasTests>>();
            var lambdas = AppHost.Services.GetRequiredService<Lambdas>();

            var result = await lambdas.GetQueue(logging, 6, 6);

            result.Should()
                .NotBeNullOrEmpty();

            result.Length.Should()
                .Be(6);

        }
    }
}
namespace Homeschool.Aws.Client;

using System.Diagnostics;
using System.Reflection;

using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime.Internal;

using DomainModels.Courses;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

public class Lambdas
{
    public IConfiguration Config
    {
        get;
    }

    private AmazonLambdaClient _client;

    public Lambdas(IConfiguration config)
    {
        Config = config;
        _client = new(
            config["awsaccessKeyID"],
            config["awsSecreteAccessKey"],
            RegionEndpoint.USEast2
        );
    }

    public async Task<LessonQueueItem[]> GetQueue(ILogger logger, int min, int max)
    {
        logger.LogDebug($"{MethodBase.GetCurrentMethod().Name}: Enter");

        try
        {
            InvokeRequest ir = new()
            {
                FunctionName = "HomeschoolGetQueue",
                InvocationType = InvocationType.RequestResponse,
                Payload
                    = $@"{{ ""body"": ""{{ \""min\"": {min}, \""max\"": {max} }}"" }}",
            };

            InvokeResponse response = await _client.InvokeAsync(ir, CancellationToken.None);

            StreamReader sr = new(response.Payload);
            Debug.WriteLine(sr.ReadToEnd());
            response.Payload.Seek(0, SeekOrigin.Begin);
            JsonTextReader reader = new(sr);

            JsonSerializer serializer = new();
            var op = serializer.Deserialize<LessonQueueItem[]>(reader);

            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}: {op}");

            return op;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{MethodBase.GetCurrentMethod().Name}: Caught Exception");

            throw;
        }
        finally
        {
            logger.LogDebug($"{MethodBase.GetCurrentMethod().Name}: Finally (Leaving)");
        }
    }

    public async Task<LessonQueueItem[]?> MarkLessonCompleted(
        Guid nextLessonLessonUid,
        DateTimeOffset nextLessonMarkedCompleteDateTime,
        ILogger logger = null
    )
    {
        logger.LogDebug($"{MethodBase.GetCurrentMethod().Name}: Enter");

        try
        {
            InvokeRequest ir = new()
            {
                FunctionName = "MarkLessonCompleted",
                InvocationType = InvocationType.RequestResponse,
                Payload = $@"{{ ""body"": ""{nextLessonLessonUid}"" }}",
            };

            InvokeResponse response = await _client.InvokeAsync(ir, CancellationToken.None);

            StreamReader sr = new(response.Payload);
            JsonTextReader reader = new(sr);

            JsonSerializer serializer = new();
            LessonQueueItem[]? op = serializer.Deserialize<LessonQueueItem[]>(reader);

            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}: {op}");

            return op;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{MethodBase.GetCurrentMethod().Name}: Caught Exception");

            throw;
        }
        finally
        {
            logger.LogDebug($"{MethodBase.GetCurrentMethod().Name}: Finally (Leaving)");
        }
    }

    public async Task<LessonQueueItem[]?> MarkLessonOpened(
        Guid nextLessonLessonUid,
        DateTimeOffset nextLessonMarkedCompleteDateTime,
        ILogger logger = null
    )
    {
        logger.LogDebug($"{MethodBase.GetCurrentMethod().Name}: Enter");

        try
        {
            InvokeRequest ir = new()
            {
                FunctionName = "MarkLessonOpened",
                InvocationType = InvocationType.RequestResponse,
                Payload = $@"{{ ""body"": ""{nextLessonLessonUid}"" }}",
            };

            InvokeResponse response = await _client.InvokeAsync(ir, CancellationToken.None);

            StreamReader sr = new(response.Payload);
            JsonTextReader reader = new(sr);

            JsonSerializer serializer = new();
            LessonQueueItem[]? op = serializer.Deserialize<LessonQueueItem[]>(reader);

            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}: {op}");

            return op;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{MethodBase.GetCurrentMethod().Name}: Caught Exception");

            throw;
        }
        finally
        {
            logger.LogDebug($"{MethodBase.GetCurrentMethod().Name}: Finally (Leaving)");
        }
    }
}

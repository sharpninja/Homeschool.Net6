namespace Homeschool.Proxy;

public class WcfSettings
{
    private ILogger<WcfSettings> Logger { get; }

    public WcfSettings(IServiceProvider provider)
    {
        var config = provider.GetRequiredService<IConfiguration>();
        Logger = provider.GetRequiredService<ILogger<WcfSettings>>();
        Path = provider.GetRequiredService<PathName>().Path;
        IConfigurationSection? settings = config.GetSection("WcfSettings");

        if (settings is null)
        {
            return;
        }

        foreach (var child in settings.GetChildren())
        {
            //if (Debugger.IsAttached)
            //{
            //    Debugger.Break();
            //}

            switch (child.Key)
            {
                case nameof(WcfSettings.Hostname):
                    Hostname = child.Value;


                    //if (Debugger.IsAttached)
                    //{
                    //    Debugger.Break();
                    //}

                    break;

                case  nameof(WcfSettings.HttpPort):
                    HttpPort = int.Parse(child.Value);

                    //if (Debugger.IsAttached)
                    //{
                    //    Debugger.Break();
                    //}

                    break;

                case  nameof(WcfSettings.HttpsPort):
                    HttpsPort = int.Parse(child.Value);

                    //if (Debugger.IsAttached)
                    //{
                    //    Debugger.Break();
                    //}

                    break;

                case  nameof(WcfSettings.NettcpPort):
                    NettcpPort = int.Parse(child.Value);

                    //if (Debugger.IsAttached)
                    //{
                    //    Debugger.Break();
                    //}

                    break;

                case  nameof(WcfSettings.UseHttps):
                    UseHttps = bool.Parse(child.Value);

                    //if (Debugger.IsAttached)
                    //{
                    //    Debugger.Break();
                    //}

                    break;
            }
        }

        Logger.LogInformation(ToJson());
    }

    private const string DEFAULT_HOST_NAME = "localhost";

    [ JsonInclude ]
    public string Hostname { get; set; } = WcfSettings.DEFAULT_HOST_NAME;

    [JsonInclude]
    public bool UseHttps { get; set; } = true;

    [JsonIgnore]
    public Uri BasicHttpAddress { get; set; } = new Uri("http://localhost");

    [ JsonIgnore ]
    public Uri BasicHttpsAddress { get; set; } = new Uri("https://localhost");

    [ JsonIgnore ]
    public Uri? WsHttpAddress { get; set; }

    [ JsonIgnore ]
    public Uri? WsHttpAddressValidateUserPassword { get; set; }

    [ JsonIgnore ]
    public Uri? WsHttpsAddress { get; set; }

    [ JsonIgnore ]
    public Uri? NetTcpAddress { get; set; }

    [ JsonInclude ]
    public int HttpPort { get; set; } = 5000; //8088;

    [ JsonInclude ]
    public int HttpsPort { get; set; } = 5001; // 8443;

    [ JsonInclude ]
    public int NettcpPort { get; set; } = 8089;

    public static string GetSettingsFilename(string path) =>
        System.IO.Path.Combine(path, "wcfSettings.json");

    public IEnumerable<Uri> GetBaseAddresses()
    {
        return new[] {
                //$"net.tcp://{hostname}:{NettcpPort}/",
                $"http://{Hostname}:{HttpPort}/",
                //$"https://{hostname}:{HttpsPort}/"
            }
        .Select(static a => new Uri(a)).ToArray();
    }

    private static Uri AddPathPrefix(Uri source, string? prefix)
    {
        if (string.IsNullOrEmpty(prefix))
        {
            return source;
        }

        UriBuilder builder = new (source);
        builder.Path = prefix + builder.Path;
        return builder.Uri;

    }


    private string Path { get; }

    public WcfSettings Initialize(
        string? servicePrefix = default,
        string? path = null)
    {
        var baseHttpAddress = $"{Hostname}:{HttpPort}";
        var baseHttpsAddress = $"{Hostname}:{HttpsPort}";
        var baseTcpAddress = $"{Hostname}:{NettcpPort}";

        BasicHttpAddress = AddPathPrefix(
            new Uri($"http://{baseHttpAddress}/basichttp"),
            servicePrefix
        );

        if (UseHttps)
        {
            BasicHttpsAddress = AddPathPrefix(
                new Uri($"https://{baseHttpsAddress}/basichttp"),
                servicePrefix
            );
        }

        //WsHttpAddress = AddPathPrefix(
        //    new Uri($"http://{baseHttpAddress}/wsHttp"),
        //    servicePrefix
        //);
        //WsHttpAddressValidateUserPassword = AddPathPrefix(
        //    new Uri($"https://{baseHttpsAddress}/wsHttpUserPassword"),
        //    servicePrefix
        //);
        //WsHttpsAddress = AddPathPrefix(
        //    new Uri($"https://{baseHttpsAddress}/wsHttp"),
        //    servicePrefix
        //);
        //NetTcpAddress = AddPathPrefix(
        //    new Uri($"net.tcp://{baseTcpAddress}/nettcp"),
        //    servicePrefix
        //);

        path ??= Path;

        if (path is
            {
                Length: > 0
            })
        {
            var filename = GetSettingsFilename(path);
            File.WriteAllText(filename, ToJson());
            Logger.LogInformation($"Wrote [{filename}]");
        }

        return this;
    }

    public string ToJson(JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            MaxDepth = 3,
            AllowTrailingCommas = false,
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.WriteAsString,
            PropertyNameCaseInsensitive = false,
        };
        return JsonSerializer.Serialize(new { WcfSettings=this, }, options);
    }
}
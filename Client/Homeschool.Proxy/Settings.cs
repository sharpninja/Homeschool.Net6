namespace Homeschool.Proxy;

using System;
using System.Collections.Generic;
using System.Linq;

public class Settings
{
    private const string DEFAULT_HOST_NAME = "localhost";
    public bool UseHttps { get; set; } = true;
    public Uri? BasicHttpAddress { get; set; }
    public Uri? BasicHttpsAddress { get; set; }
    public Uri? WsHttpAddress { get; set; }
    public Uri? WsHttpAddressValidateUserPassword { get; set; }
    public Uri? WsHttpsAddress { get; set; }
    public Uri? NetTcpAddress { get; set; }

    public int HttpPort { get; set; } = 8088;
    public int HttpsPort { get; set; } = 8443;
    public int NettcpPort { get; set; } = 8089;

    public IEnumerable<Uri> GetBaseAddresses(string hostname = DEFAULT_HOST_NAME)
    {
        return new[] {
                $"net.tcp://{hostname}:{NettcpPort}/",
                $"http://{hostname}:{HttpPort}/",
                $"https://{hostname}:{HttpsPort}/" }
        .Select(static a => new Uri(a)).ToArray();
    }

    private static Uri? AddPathPrefix(Uri? source, string? prefix)
    {
        if (string.IsNullOrEmpty(prefix))
        {
            return source;
        }

        if (source is null)
        {
            return default;
        }

        var builder = new UriBuilder(source);
        builder.Path = prefix + builder.Path;
        return builder.Uri;

    }


    public Settings SetDefaults(
        string hostname = DEFAULT_HOST_NAME,
        string? servicePrefix = default)
    {
        var baseHttpAddress = hostname + ":8088";
        var baseHttpsAddress = hostname + ":8443";
        var baseTcpAddress = hostname + ":8089";

        BasicHttpAddress = AddPathPrefix(
            new Uri($"http://{baseHttpAddress}/basichttp"),
            servicePrefix
        );
        BasicHttpsAddress = AddPathPrefix(
            new Uri($"https://{baseHttpsAddress}/basichttp"),
            servicePrefix
        );
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
        return this;
    }
}

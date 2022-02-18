// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration(
    configurationBuilder =>
    {
        configurationBuilder.AddJsonFile("appsettings.json", false);
    }
);
builder.ConfigureLogging(
    loggingBuilder =>
    {
        loggingBuilder.AddDebug();
        loggingBuilder.AddConsole();
    }
);
builder.ConfigureServices(
    collection =>
    {
        collection.AddTransient(
            provider =>
            {
                Homeschool.Proxy.WcfProxy wcfProxy = new();
                wcfProxy.Initialize(provider);

                return wcfProxy;
            }
        );
    }
);

var host = builder.Build();

const int DELAY = 3;
Console.WriteLine($"Waiting {DELAY} seconds.");
await Task.Delay(TimeSpan.FromSeconds(DELAY));
Console.WriteLine("Starting proxy.");
var proxy = host.Services.GetService<Homeschool.Proxy.WcfProxy>()!;

Console.WriteLine("Invoking proxy.");
var result = proxy.Test();

Console.WriteLine("Displaying results.");

Console.WriteLine(result);

Console.ReadKey();

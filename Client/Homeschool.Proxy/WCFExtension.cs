namespace Homeschool.Proxy;

using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

public static class WCFExtension
{
    public static async Task<TResult> WcfInvokeAsync<TContract, TResult>(this ChannelFactory<TContract> factory, Func<TContract, Task<TResult>> action)
    {
        TContract client = factory.CreateChannel();

        if (client is null)
        {
            throw new ApplicationException("Could not create client.");
        }

        IClientChannel clientInstance = ((IClientChannel)client);

        try
        {
            TResult result = await action(client);
            clientInstance.Close();
            return result;
        }
        catch (CommunicationException)
        {
            clientInstance.Abort();
            throw;
        }
        catch (TimeoutException)
        {
            clientInstance.Abort();
            throw;
        }
    }

    public static async Task<TResult> WcfInvokeAsync<TContract, TResult>(this Func<TContract, Task<TResult>> wcfAction,
        Binding binding, Uri url, Action<ChannelFactory<TContract>>? factorySetup = null)
    {
        binding.ApplyDebugTimeouts();
        var factory = new ChannelFactory<TContract>(binding, new EndpointAddress(url));
        factorySetup?.Invoke(factory);

        factory.Open();
        try
        {
            return await factory.WcfInvokeAsync(wcfAction);
        }
        finally
        {
            factory.Close();
        }
    }


    public static Task<TResult> WcfInvokeAsync<TContract, TResult>(this ChannelFactory<TContract> factory, Func<TContract, TResult> action)
    {
        TContract client = factory.CreateChannel();

        if (client is null)
        {
            throw new ApplicationException("Could not create client.");
        }

        IClientChannel clientInstance = (IClientChannel)client;

        TResult? result = default;
        try
        {
            result = action(client);
            clientInstance.Close();
            return Task.FromResult<TResult>(result!);
        }
        catch (CommunicationException)
        {
            clientInstance.Abort();
            throw;
        }
        catch (TimeoutException)
        {
            clientInstance.Abort();
            throw;
        }
    }

    public static Task<TResult> WcfInvokeAsync<TContract, TResult>(this Func<TContract, TResult> wcfAction,
        Binding binding, Uri url, Action<ChannelFactory<TContract>>? factorySetup = null)
    {
        binding.ApplyDebugTimeouts();

        var factory = new ChannelFactory<TContract>(
            binding, new EndpointAddress(url));
        factorySetup?.Invoke(factory);

        factory.Open();
        try
        {
            return factory?.WcfInvokeAsync(wcfAction);
        }
        finally
        {
            factory.Close();
        }
    }

    private static readonly TimeSpan s_debugTimeout = TimeSpan.FromMinutes(20);

    [Conditional("DEBUG")]
    public static void ApplyDebugTimeouts(this Binding binding, TimeSpan debugTimeout = default)
    {
        if (Debugger.IsAttached)
        {
            debugTimeout = default == debugTimeout ? WCFExtension.s_debugTimeout : debugTimeout;
            binding.OpenTimeout =
                binding.CloseTimeout =
                binding.SendTimeout =
                binding.ReceiveTimeout = debugTimeout;
        }
    }

}

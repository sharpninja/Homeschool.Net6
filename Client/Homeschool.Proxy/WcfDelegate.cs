namespace Homeschool.Proxy;

public delegate TResult? WcfDelegate<in TContract, out TResult>(TContract channel, params string[] parameters);

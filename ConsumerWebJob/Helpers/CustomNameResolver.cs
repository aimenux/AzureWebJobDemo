using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace ConsumerWebJob.Helpers;

public class CustomNameResolver : INameResolver
{
    private readonly IConfiguration _configuration;

    private static readonly IDictionary<string, string> Cache = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public CustomNameResolver(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string Resolve(string name)
    {
        if (Cache.TryGetValue(name, out var value)) return value;
        value = _configuration.GetValue<string>(name);
        Cache.TryAdd(name, value);
        return value;
    }
}
// <copyright file="PluginSourceResolver.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

using System.Reflection;
using System.Text.Json;

/// <summary>
/// Resolves a raw source string (alias, alias/id, URL, or local path) to a
/// fully-qualified base URL or <c>embedded://</c> path using a deterministic
/// six-step chain.
/// </summary>
public class PluginSourceResolver
{
    private readonly OlavConfig localConfig;
    private readonly GlobalConfig globalConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginSourceResolver"/> class.
    /// </summary>
    /// <param name="localConfig">Local project sources from <c>olav.json</c>.</param>
    /// <param name="globalConfig">Global machine sources from <c>~/.olav/config.json</c>.</param>
    public PluginSourceResolver(OlavConfig localConfig, GlobalConfig globalConfig)
    {
        this.localConfig = localConfig;
        this.globalConfig = globalConfig;
    }

    /// <summary>
    /// Resolves <paramref name="rawSource"/> and returns the canonical base URL
    /// (or <c>embedded://id</c> for built-in plugins).
    /// </summary>
    /// <param name="rawSource">Raw source string (alias, URL, or local path).</param>
    /// <returns>The fully-qualified base URL or embedded scheme identifier.</returns>
    /// <exception cref="InvalidOperationException">Thrown when resolution fails.</exception>
    public string Resolve(string rawSource)
    {
        if (IsAbsoluteUrl(rawSource) || IsLocalPath(rawSource))
        {
            return rawSource;
        }

        int slash = rawSource.IndexOf('/');
        if (slash > 0)
        {
            string alias = rawSource.Substring(0, slash);
            string pluginId = rawSource.Substring(slash + 1);
            string baseUrl = this.ResolveAlias(alias);
            return baseUrl.TrimEnd('/') + "/" + pluginId;
        }

        return this.ResolveAlias(rawSource);
    }

    private static bool IsAbsoluteUrl(string s)
    {
        return s.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
               s.StartsWith("http://", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsLocalPath(string s)
    {
        return s.StartsWith("./", StringComparison.Ordinal) ||
               s.StartsWith("/", StringComparison.Ordinal) ||
               Path.IsPathRooted(s);
    }

    private static Dictionary<string, Dictionary<string, string>> LoadBuiltInRegistry()
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        using Stream stream = asm.GetManifestResourceStream("plugin-registry.json")
            ?? throw new InvalidOperationException("Built-in plugin registry resource not found.");
        using StreamReader reader = new StreamReader(stream);
        return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(reader.ReadToEnd())
            ?? new Dictionary<string, Dictionary<string, string>>();
    }

    private string ResolveAlias(string alias)
    {
        if (this.localConfig.Sources != null &&
            this.localConfig.Sources.TryGetValue(alias, out string? localUrl))
        {
            return localUrl;
        }

        if (this.globalConfig.Sources != null &&
            this.globalConfig.Sources.TryGetValue(alias, out string? globalUrl))
        {
            return globalUrl;
        }

        Dictionary<string, Dictionary<string, string>> registry = LoadBuiltInRegistry();
        foreach (Dictionary<string, string> category in registry.Values)
        {
            if (category.TryGetValue(alias, out string? builtInUrl))
            {
                return builtInUrl;
            }
        }

        throw new PluginSourceNotFoundException(alias);
    }
}

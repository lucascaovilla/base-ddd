// <copyright file="PluginManifestFetcher.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

using System.Net.Http;
using System.Reflection;
using System.Text.Json;

/// <summary>
/// Fetches and deserialises an <c>olav.plugin.json</c> manifest from a
/// base URL, a local path, or an embedded (bundled) resource.
/// </summary>
public static class PluginManifestFetcher
{
    private static readonly HttpClient HttpClient = new HttpClient();

    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Fetches the manifest for the plugin at <paramref name="baseUrl"/>.
    /// <list type="bullet">
    ///   <item><c>embedded://id</c> → loads bundled embedded resource.</item>
    ///   <item>Local path (starts with <c>./</c>, <c>/</c>, or absolute) → reads from disk.</item>
    ///   <item>Otherwise → HTTP GET to <c>{baseUrl}/olav.plugin.json</c>.</item>
    /// </list>
    /// </summary>
    /// <param name="baseUrl">Base URL, local path, or embedded scheme identifier.</param>
    /// <returns>The deserialised <see cref="OlavPluginManifest"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown with a <c>✗</c> message on any failure.</exception>
    public static OlavPluginManifest Fetch(string baseUrl)
    {
        if (baseUrl.StartsWith("embedded://", StringComparison.OrdinalIgnoreCase))
        {
            return LoadEmbedded(baseUrl.Substring("embedded://".Length));
        }

        if (IsLocalPath(baseUrl))
        {
            return LoadFromLocalPath(baseUrl);
        }

        return FetchFromUrl(baseUrl);
    }

    private static bool IsLocalPath(string s)
    {
        return s.StartsWith("./", StringComparison.Ordinal) ||
               s.StartsWith("/", StringComparison.Ordinal) ||
               Path.IsPathRooted(s);
    }

    private static OlavPluginManifest LoadEmbedded(string pluginId)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = pluginId + ".manifest.json";
        using Stream stream = asm.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"✗ Bundled manifest not found for plugin '{pluginId}'. " +
                $"Ensure the resource '{resourceName}' is embedded in the assembly.");
        using StreamReader reader = new StreamReader(stream);
        return Deserialize(reader.ReadToEnd(), "embedded:" + pluginId);
    }

    private static OlavPluginManifest LoadFromLocalPath(string basePath)
    {
        string manifestPath = basePath.EndsWith("olav.plugin.json", StringComparison.OrdinalIgnoreCase)
            ? basePath
            : Path.Combine(basePath, "olav.plugin.json");

        if (!File.Exists(manifestPath))
        {
            throw new InvalidOperationException(
                $"✗ Manifest not found at '{manifestPath}'.");
        }

        return Deserialize(File.ReadAllText(manifestPath), manifestPath);
    }

    private static OlavPluginManifest FetchFromUrl(string baseUrl)
    {
        string manifestUrl = baseUrl.TrimEnd('/') + "/olav.plugin.json";

        try
        {
            string json = HttpClient.GetStringAsync(manifestUrl).GetAwaiter().GetResult();
            return Deserialize(json, manifestUrl);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"✗ Failed to fetch manifest from '{manifestUrl}': {ex.Message}");
        }
    }

    private static OlavPluginManifest Deserialize(string json, string source)
    {
        try
        {
            return JsonSerializer.Deserialize<OlavPluginManifest>(json, SerializerOptions)
                ?? throw new InvalidOperationException($"✗ Manifest at '{source}' deserialised to null.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"✗ Failed to parse manifest from '{source}': {ex.Message}");
        }
    }
}

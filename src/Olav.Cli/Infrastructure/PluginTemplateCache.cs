// <copyright file="PluginTemplateCache.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

using System.Net.Http;
using System.Reflection;

/// <summary>
/// Resolves plugin template content from an embedded resource, a local directory,
/// or a remote URL (caching remote templates to <c>~/.olav/cache/</c>).
/// </summary>
public static class PluginTemplateCache
{
    private static readonly HttpClient HttpClient = new HttpClient();

    /// <summary>
    /// Returns the raw content of <paramref name="templateName"/> for the plugin
    /// located at <paramref name="manifestBaseUrl"/>.
    /// </summary>
    /// <param name="manifestBaseUrl">The resolved manifest base URL or embedded scheme identifier.</param>
    /// <param name="templateName">The template filename (e.g. <c>azure-deploy.yml.sbn</c>).</param>
    /// <param name="pluginId">The plugin identifier (used for cache path).</param>
    /// <param name="pluginVersion">The plugin version (used for cache path).</param>
    /// <returns>Raw template content as a string.</returns>
    public static string GetTemplateContent(
        string manifestBaseUrl,
        string templateName,
        string pluginId,
        string pluginVersion)
    {
        if (manifestBaseUrl.StartsWith("embedded://", StringComparison.OrdinalIgnoreCase))
        {
            string plugin = manifestBaseUrl.Substring("embedded://".Length);
            return LoadEmbeddedTemplate(plugin, templateName);
        }

        if (IsLocalPath(manifestBaseUrl))
        {
            return LoadLocalTemplate(manifestBaseUrl, templateName);
        }

        return FetchRemoteTemplate(manifestBaseUrl, templateName, pluginId, pluginVersion);
    }

    private static bool IsLocalPath(string s)
    {
        return s.StartsWith("./", StringComparison.Ordinal) ||
               s.StartsWith("/", StringComparison.Ordinal) ||
               Path.IsPathRooted(s);
    }

    private static string GetCacheDir(string pluginId, string pluginVersion)
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(home, ".olav", "cache", pluginId, pluginVersion, "templates");
    }

    private static string LoadEmbeddedTemplate(string pluginId, string templateName)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = pluginId + "." + templateName;
        using Stream stream = asm.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"✗ Bundled template '{templateName}' not found for plugin '{pluginId}'.");
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static string LoadLocalTemplate(string basePath, string templateName)
    {
        string templatesDir = Path.Combine(basePath, "templates");
        string templatePath = Path.Combine(templatesDir, templateName);

        if (!File.Exists(templatePath))
        {
            throw new InvalidOperationException(
                $"✗ Template '{templateName}' not found at '{templatePath}'.");
        }

        return File.ReadAllText(templatePath);
    }

    private static string FetchRemoteTemplate(
        string manifestBaseUrl,
        string templateName,
        string pluginId,
        string pluginVersion)
    {
        string cacheDir = GetCacheDir(pluginId, pluginVersion);
        string cachedPath = Path.Combine(cacheDir, templateName);

        if (File.Exists(cachedPath))
        {
            return File.ReadAllText(cachedPath);
        }

        string url = manifestBaseUrl.TrimEnd('/') + "/templates/" + templateName;

        try
        {
            string content = HttpClient.GetStringAsync(url).GetAwaiter().GetResult();
            Directory.CreateDirectory(cacheDir);
            File.WriteAllText(cachedPath, content);
            return content;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"✗ Failed to fetch template '{templateName}' from '{url}': {ex.Message}");
        }
    }
}

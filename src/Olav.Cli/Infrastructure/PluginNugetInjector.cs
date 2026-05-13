// <copyright file="PluginNugetInjector.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

using System.Xml.Linq;

/// <summary>
/// Adds <c>PackageReference</c> entries to <c>.csproj</c> files and
/// <c>packageSource</c> entries to <c>nuget.config</c> files.
/// Both operations are idempotent — they do nothing when the entry already exists.
/// </summary>
public static class PluginNugetInjector
{
    /// <summary>
    /// Adds a <c>&lt;PackageReference&gt;</c> to the given <c>.csproj</c>.
    /// No-ops if the reference is already present.
    /// </summary>
    /// <param name="csprojPath">Absolute path to the <c>.csproj</c> file.</param>
    /// <param name="packageId">The NuGet package identifier.</param>
    /// <param name="version">The package version to reference.</param>
    public static void AddPackageReference(string csprojPath, string packageId, string version)
    {
        if (!File.Exists(csprojPath))
        {
            throw new InvalidOperationException(
                $"✗ Project file not found: '{csprojPath}'.");
        }

        XDocument doc = XDocument.Load(csprojPath);

        bool exists = doc.Descendants("PackageReference")
            .Any(e => string.Equals(
                e.Attribute("Include")?.Value,
                packageId,
                StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            return;
        }

        XElement? itemGroup = doc.Descendants("ItemGroup")
            .FirstOrDefault(g => g.Elements("PackageReference").Any());

        if (itemGroup == null)
        {
            itemGroup = new XElement("ItemGroup");
            doc.Root!.Add(itemGroup);
        }

        itemGroup.Add(
            new XElement(
                "PackageReference",
                new XAttribute("Include", packageId),
                new XAttribute("Version", version)));

        doc.Save(csprojPath);
    }

    /// <summary>
    /// Adds a <c>&lt;add key="…" value="…"/&gt;</c> entry to the
    /// <c>packageSources</c> section of <c>nuget.config</c>.
    /// Creates the file if it does not exist.
    /// No-ops if a source with the same key or URL already exists.
    /// </summary>
    /// <param name="nugetConfigPath">Absolute path to <c>nuget.config</c>.</param>
    /// <param name="sourceName">Key name for the new package source.</param>
    /// <param name="url">Feed URL for the new package source.</param>
    public static void AddPackageSource(string nugetConfigPath, string sourceName, string url)
    {
        XDocument doc = File.Exists(nugetConfigPath)
            ? XDocument.Load(nugetConfigPath)
            : BuildDefaultNugetConfig();

        XElement? packageSources = doc.Descendants("packageSources").FirstOrDefault();
        if (packageSources == null)
        {
            packageSources = new XElement("packageSources");
            doc.Root!.Add(packageSources);
        }

        bool exists = packageSources.Elements("add").Any(e =>
            string.Equals(e.Attribute("key")?.Value, sourceName, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(e.Attribute("value")?.Value, url, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            return;
        }

        packageSources.Add(
            new XElement(
                "add",
                new XAttribute("key", sourceName),
                new XAttribute("value", url)));

        string? dir = Path.GetDirectoryName(nugetConfigPath);
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }

        doc.Save(nugetConfigPath);
    }

    private static XDocument BuildDefaultNugetConfig()
    {
        return new XDocument(
            new XElement(
                "configuration",
                new XElement(
                    "packageSources",
                    new XElement(
                        "add",
                        new XAttribute("key", "nuget.org"),
                        new XAttribute("value", "https://api.nuget.org/v3/index.json")))));
    }
}

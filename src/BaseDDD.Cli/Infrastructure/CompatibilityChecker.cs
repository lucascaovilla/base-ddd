// <copyright file="CompatibilityChecker.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Infrastructure;

/// <summary>
/// Checks a project's template version against the tool's supported range.
/// </summary>
public static class CompatibilityChecker
{
    /// <summary>
    /// Checks compatibility of the given project template version.
    /// Warns to stdout for outdated versions, throws for unsupported ones.
    /// </summary>
    /// <param name="projectTemplateVersion">The templateVersion value from the project's baseddd.json.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the version is below minimum supported or newer than the tool knows.
    /// </exception>
    public static void Check(string projectTemplateVersion)
    {
        Version project = Parse(projectTemplateVersion);
        Version current = Parse(VersionConstants.TemplateVersion);
        Version min = Parse(VersionConstants.MinSupportedTemplateVersion);

        if (project > current)
        {
            throw new InvalidOperationException(
                $"This project uses template version {projectTemplateVersion}, " +
                $"but this tool only supports up to {VersionConstants.TemplateVersion}. " +
                $"Please update the BaseDDD CLI.");
        }

        if (project < min)
        {
            throw new InvalidOperationException(
                $"This project uses template version {projectTemplateVersion}, " +
                $"which is below the minimum supported version {VersionConstants.MinSupportedTemplateVersion}. " +
                $"Run 'baseddd migrate --apply' to upgrade before continuing.");
        }

        if (project < current)
        {
            Console.WriteLine(
                $"Warning: this project uses template version {projectTemplateVersion} " +
                $"(current: {VersionConstants.TemplateVersion}). " +
                $"Run 'baseddd migrate' to see what would change.");
        }
    }

    private static Version Parse(string raw)
    {
        string normalised = raw.Count(c => c == '.') switch
        {
            0 => raw + ".0.0",
            1 => raw + ".0",
            _ => raw,
        };

        return Version.TryParse(normalised, out Version? v)
            ? v
            : throw new InvalidOperationException($"Cannot parse version '{raw}'.");
    }
}

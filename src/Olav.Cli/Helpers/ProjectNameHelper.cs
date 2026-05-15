// <copyright file="ProjectNameHelper.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Helpers;

using System.IO;

/// <summary>
/// Helper to discover the project name from an Olav project structure.
/// </summary>
public static class ProjectNameHelper
{
    /// <summary>
    /// Discovers the project namespace by locating the <c>*.Infrastructure.csproj</c>
    /// file inside <c>src/</c> and stripping the <c>.Infrastructure</c> suffix.
    /// </summary>
    /// <param name="root">Repository root directory.</param>
    /// <returns>Project namespace (e.g. <c>MyApp</c>).</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no Infrastructure project is found under <c>src/</c>.
    /// </exception>
    public static string DiscoverProjectName(string root)
    {
        string srcDir = Path.Combine(root, "src");
        if (!Directory.Exists(srcDir))
        {
            throw new InvalidOperationException(
                "No 'src' directory found. Is this an Olav project?");
        }

        string[] csprojFiles = Directory.GetFiles(srcDir, "*.Infrastructure.csproj", SearchOption.AllDirectories);
        if (csprojFiles.Length == 0)
        {
            throw new InvalidOperationException(
                "No '*.Infrastructure.csproj' found under 'src/'. Is this an Olav project?");
        }

        string fileNameNoExt = Path.GetFileNameWithoutExtension(csprojFiles[0]);
        string[] parts = fileNameNoExt.Split('.');
        return parts.Length > 1
            ? string.Join(".", parts, 0, parts.Length - 1)
            : fileNameNoExt;
    }
}

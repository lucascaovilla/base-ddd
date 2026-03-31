// <copyright file="ProjectVerifier.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace Olav.Verifiers;

/// <summary>
/// Generic verifier for project structure.
/// </summary>
public static class ProjectVerifier
{
    /// <summary>
    /// Verifies existence of given file on root folder.
    /// </summary>
    /// <param name="root">Root reference folder.</param>
    /// <param name="relativePath">Relative path of to the file on root.</param>
    public static void VerifyFile(string root, string relativePath)
    {
        string fullPath = Path.Combine(root, relativePath);

        if (!File.Exists(fullPath))
        {
            throw new InvalidOperationException($"Expected file not found: {fullPath}");
        }
    }

    /// <summary>
    /// Verifies existence of given directory on root folder.
    /// </summary>
    /// <param name="root">Root reference folder.</param>
    /// <param name="relativePath">Relative path of to the directory on root.</param>
    public static void VerifyDirectory(string root, string relativePath)
    {
        string fullPath = Path.Combine(root, relativePath);

        if (!Directory.Exists(fullPath))
        {
            throw new InvalidOperationException($"Expected directory not found: {fullPath}");
        }
    }
}

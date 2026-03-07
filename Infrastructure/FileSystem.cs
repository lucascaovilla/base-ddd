// <copyright file="FileSystem.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Infrastructure;

/// <summary>
/// Provides filesystem level interaction.
/// </summary>
public static class FileSystem
{
    /// <summary>
    /// Writes a file.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <param name="content">Content to be written.</param>
    public static void WriteFile(string path, string content)
    {
        string? dir = Path.GetDirectoryName(path);

        if (dir == null)
        {
            throw new InvalidOperationException();
        }

        Directory.CreateDirectory(dir);
        File.WriteAllText(path, content);
    }

    /// <summary>
    /// Creates a directory.
    /// </summary>
    /// <param name="path">Path to the directory.</param>
    public static void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Verifies the existence of a directory.
    /// </summary>
    /// <param name="root">Root path to the directory.</param>
    /// <param name="name">Directory to check if exists.</param>
    public static void ValidateDirectory(string root, string name)
    {
        if (!Directory.Exists(Path.Combine(root, name)))
        {
            throw new InvalidOperationException($"{name} folder missing.");
        }
    }

    /// <summary>
    /// Verifies the existence of a file.
    /// </summary>
    /// <param name="root">Root path to the file.</param>
    /// <param name="name">File to check if exists.</param>
    public static void ValidateFile(string root, string name)
    {
        if (!File.Exists(Path.Combine(root, name)))
        {
            throw new InvalidOperationException($"{name} file missing.");
        }
    }

    /// <summary>
    /// Delete a file if it exists.
    /// </summary>
    /// <param name="path">Path to the file including it's name.</param>
    public static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}

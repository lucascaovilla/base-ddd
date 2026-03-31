// <copyright file="FileSystem.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

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
        try
        {
            string? directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, content);

            Console.WriteLine($"[OK] {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to write {path}");
            Console.WriteLine(ex.ToString());
            throw;
        }
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

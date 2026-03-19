// <copyright file="BaseStructureGenerator.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Generation;

using System.IO;
using BaseDDD.Infrastructure;

/// <summary>
/// Generates base folder structure for a BaseDDD project.
/// </summary>
public class BaseStructureGenerator
{
    private readonly string root;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseStructureGenerator"/> class.
    /// Class initializer.
    /// </summary>
    /// <param name="root">Repository's root.</param>
    public BaseStructureGenerator(string root)
    {
        this.root = root;
    }

    /// <summary>
    /// Centralized Generate method to properly create all base folders.
    /// </summary>
    public void Generate()
    {
        FileSystem.CreateDirectory(this.root);
        FileSystem.CreateDirectory(Path.Combine(this.root, "src"));
        FileSystem.CreateDirectory(Path.Combine(this.root, "tests"));
        FileSystem.CreateDirectory(Path.Combine(this.root, "docker"));
        FileSystem.CreateDirectory(Path.Combine(this.root, ".githooks"));
        FileSystem.CreateDirectory(Path.Combine(this.root, ".github"));
        FileSystem.CreateDirectory(Path.Combine(this.root, ".github/workflows"));
    }
}
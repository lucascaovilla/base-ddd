// <copyright file="GitGenerator.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Generation;

using Olav.Infrastructure;

/// <summary>
/// Generates base .git with initial commit.
/// </summary>
public class GitGenerator
{
    private readonly string root;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitGenerator"/> class.
    /// Class initializer.
    /// </summary>
    /// <param name="root">Repository's root.</param>
    public GitGenerator(string root)
    {
        this.root = root;
    }

    /// <summary>
    /// Centralized Generate method to properly start .git with initial commit.
    /// </summary>
    public void Generate()
    {
        GitRunner.Run("init", this.root);
        GitRunner.Run("branch -m main", this.root);
        GitRunner.Run("config core.hooksPath .githooks", this.root);

        if (!OperatingSystem.IsWindows())
        {
            ProcessRunner.Run("chmod", "+x .githooks/pre-commit", this.root);
            ProcessRunner.Run("chmod", "+x .githooks/pre-push", this.root);
        }

        GitRunner.Run("config user.email \"olav@olav.com\"", this.root);
        GitRunner.Run("config user.name \"Olav\"", this.root);

        GitRunner.Run("add .", this.root);
        GitRunner.Run("commit -m \"Initial Olav structure\"", this.root);
    }
}
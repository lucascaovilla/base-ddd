// <copyright file="IMigrationStep.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Migrations;

/// <summary>
/// A single migration step that upgrades a project from one template version to the next.
/// </summary>
public interface IMigrationStep
{
    /// <summary>Gets the template version this step migrates from.</summary>
    public string FromVersion { get; }

    /// <summary>Gets the template version this step migrates to.</summary>
    public string ToVersion { get; }

    /// <summary>
    /// Returns a human-readable description of every change this step will make.
    /// Used for dry-run output.
    /// </summary>
    /// <returns>List of changes to be applied.</returns>
    public IReadOnlyList<string> Describe();

    /// <summary>Applies the migration to the project at the given root.</summary>
    /// <param name="root">Repository's root path.</param>
    public void Apply(string root);
}

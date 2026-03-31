// <copyright file="MigrateCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;
using Olav.Helpers;

/// <summary>
/// Olav migrate generated project's version cli command.
/// </summary>
public static class MigrateCommand
{
    /// <summary>
    /// Executes migrate on project via cli tool.
    /// </summary>
    /// <param name="args">CLI arguments. Pass --apply to write changes.</param>
    public static void Execute(string[] args)
    {
        bool apply = Array.Exists(args, a => a.Equals("--apply", StringComparison.OrdinalIgnoreCase));
        string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());

        new VersionEnforcementGenerator(root).Migrate(dryRun: !apply);
    }
}

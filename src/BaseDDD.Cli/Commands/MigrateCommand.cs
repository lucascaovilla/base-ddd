// <copyright file="MigrateCommand.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Commands;

using BaseDDD.Generation;
using BaseDDD.Helpers;

/// <summary>
/// BaseDDD migrate generated project's version cli command.
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

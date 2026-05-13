// <copyright file="AddEntityCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;
using Olav.Helpers;

/// <summary>
/// Handles <c>olav add entity &lt;EntityName&gt;</c>.
/// Generates a Domain entity class.
/// </summary>
public static class AddEntityCommand
{
    /// <summary>
    /// Executes the <c>add entity</c> command.
    /// </summary>
    /// <param name="args">Full argument list (including the leading <c>add entity</c> tokens).</param>
    public static void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: olav add entity <EntityName>");
            return;
        }

        string entityName = args[2];
        string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());

        try
        {
            string projectName = ProjectNameHelper.DiscoverProjectName(root);
            new EntityGenerator(entityName, projectName, root).Generate();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

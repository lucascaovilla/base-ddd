// <copyright file="AddRepositoryCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;
using Olav.Helpers;

/// <summary>
/// Handles <c>olav add repository &lt;EntityName&gt; [--plugin &lt;id&gt;]</c>.
/// Generates a Domain repository interface and its Infrastructure implementation.
/// </summary>
public static class AddRepositoryCommand
{
    /// <summary>
    /// Executes the <c>add repository</c> command.
    /// </summary>
    /// <param name="args">Full argument list (including the leading <c>add repository</c> tokens).</param>
    public static void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: olav add repository <EntityName> [<plugin>]");
            return;
        }

        string entityName = args[2];
        string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());
        string? plugin = args.Length > 3 ? args[3] : InstalledPluginHelper.ResolveInfrastructurePlugin(root);

        try
        {
            string projectName = ProjectNameHelper.DiscoverProjectName(root);
            new RepositoryGenerator(entityName, projectName, root, plugin).Generate();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

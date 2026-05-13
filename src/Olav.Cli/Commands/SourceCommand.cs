// <copyright file="SourceCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;
using Olav.Helpers;

/// <summary>
/// Handles <c>olav source add &lt;alias&gt; &lt;url&gt;</c>,
/// <c>olav source remove &lt;alias&gt;</c>, and <c>olav source list</c>.
/// </summary>
public static class SourceCommand
{
    /// <summary>
    /// Executes a source sub-command.
    /// </summary>
    /// <param name="args">Full argument list (including the leading <c>source</c> token).</param>
    public static void Execute(string[] args)
    {
        string sub = args.Length > 1 ? args[1].ToLowerInvariant() : string.Empty;

        switch (sub)
        {
            case "add":
                ExecuteAdd(args);
                break;

            case "remove":
                ExecuteRemove(args);
                break;

            case "list":
                ExecuteList();
                break;

            default:
                PrintHelp();
                break;
        }
    }

    private static void ExecuteAdd(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: olav source add <alias> <url>");
            return;
        }

        try
        {
            PluginManagementGenerator.AddGlobalSource(args[2], args[3]);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void ExecuteRemove(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: olav source remove <alias>");
            return;
        }

        try
        {
            PluginManagementGenerator.RemoveGlobalSource(args[2]);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void ExecuteList()
    {
        try
        {
            string root = TryFindProjectRoot();
            PluginManagementGenerator manager = new PluginManagementGenerator(root);
            IReadOnlyList<SourceInfo> sources = manager.ListSources();

            Console.WriteLine(
                string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{0,-20} {1,-50} {2}",
                    "NAME",
                    "URL",
                    "SCOPE"));
            Console.WriteLine(new string('-', 80));
            foreach (SourceInfo source in sources)
            {
                Console.WriteLine(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "{0,-20} {1,-50} [{2}]",
                        source.Name,
                        source.Url,
                        source.Scope));
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static string TryFindProjectRoot()
    {
        try
        {
            return ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());
        }
        catch
        {
            return Directory.GetCurrentDirectory();
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  olav source add <alias> <url>");
        Console.WriteLine("  olav source remove <alias>");
        Console.WriteLine("  olav source list");
    }
}

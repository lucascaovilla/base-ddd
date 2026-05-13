// <copyright file="PluginCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;
using Olav.Helpers;

/// <summary>
/// Handles <c>olav plugin list</c> and <c>olav plugin remove &lt;id&gt;</c>.
/// </summary>
public static class PluginCommand
{
    /// <summary>
    /// Executes a plugin sub-command.
    /// </summary>
    /// <param name="args">Full argument list (including the leading <c>plugin</c> token).</param>
    public static void Execute(string[] args)
    {
        string sub = args.Length > 1 ? args[1].ToLowerInvariant() : string.Empty;

        switch (sub)
        {
            case "list":
                ExecuteList();
                break;

            case "remove":
                ExecuteRemove(args);
                break;

            default:
                PrintHelp();
                break;
        }
    }

    private static void ExecuteList()
    {
        try
        {
            string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());
            PluginManagementGenerator manager = new PluginManagementGenerator(root);
            IReadOnlyList<InstalledPluginInfo> plugins = manager.ListPlugins();

            if (plugins.Count == 0)
            {
                Console.WriteLine("No plugins installed.");
                return;
            }

            Console.WriteLine(
                string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{0,-20} {1,-10} {2,-16} {3,-12} {4}",
                    "ID",
                    "VERSION",
                    "CATEGORY",
                    "DELIVERY",
                    "SOURCE"));
            Console.WriteLine(new string('-', 80));
            foreach (InstalledPluginInfo plugin in plugins)
            {
                Console.WriteLine(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "{0,-20} {1,-10} {2,-16} {3,-12} {4}",
                        plugin.Id,
                        plugin.Version,
                        plugin.Category,
                        plugin.Delivery,
                        plugin.Source));
            }
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
            Console.WriteLine("Usage: olav plugin remove <id>");
            return;
        }

        string pluginId = args[2];

        try
        {
            string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());
            PluginManagementGenerator manager = new PluginManagementGenerator(root);
            manager.RemovePlugin(pluginId);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  olav plugin list");
        Console.WriteLine("  olav plugin remove <id>");
    }
}

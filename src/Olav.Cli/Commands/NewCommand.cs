// <copyright file="NewCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Extensions;
using Olav.Generation;

/// <summary>
/// Olav new project cli command.
/// </summary>
public static class NewCommand
{
    /// <summary>
    /// Executes new command to generate a clean new project or plugin scaffold.
    /// </summary>
    /// <param name="args">Arguments to the new command.</param>
    public static void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: olav new <ProjectName> | olav new plugin <id>");
            return;
        }

        if (args[1].Equals("plugin", StringComparison.OrdinalIgnoreCase))
        {
            ExecutePluginScaffold(args);
            return;
        }

        string name = args[1];
        string owner = GetOption(args, "--owner") ?? Environment.UserName;
        string license = GetOption(args, "--license") ?? "MIT";

        string root = Path.Combine(Directory.GetCurrentDirectory(), name.ToDashCase());

        if (Directory.Exists(root))
        {
            Console.WriteLine("Directory already exists.");
            return;
        }

        Console.WriteLine($"Creating Olav project: {name}");

        int total = 8;
        void Step(int n, string label, Action run)
        {
            string prefix = $"  [{n}/{total}] {label}";
            Console.Write(prefix + "...");
            run();
            if (!Console.IsOutputRedirected)
            {
                Console.Write($"\r{prefix}... ✓\n");
            }
            else
            {
                Console.WriteLine(" ✓");
            }
        }

        Step(1, "Creating project structure", () => new BaseStructureGenerator(root).Generate());
        Step(2, "Writing version info", () => new VersionEnforcementGenerator(root).Generate());
        Step(3, "Scaffolding projects", () => new ProjectGenerator(name, root).Generate());
        Step(4, "Scaffolding test projects", () => new TestGenerator(name, root).Generate());
        Step(5, "Generating solution", () => new SolutionGenerator(name, root).Generate());
        Step(6, "Generating template files", () => new FileTemplateGenerator(name, root, owner, license).Generate());
        Step(7, "Wiring observability", () => new ObservabilityGenerator(name, root, owner, license).Generate());
        Step(8, "Initializing git", () => new GitGenerator(root).Generate());

        Console.WriteLine("Olav solution created successfully.");
    }

    private static void ExecutePluginScaffold(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: olav new plugin <id> [--category <value>] [--delivery <value>] [--author <value>]");
            return;
        }

        string pluginId = args[2];
        string category = GetOption(args, "--category") ?? "infrastructure";
        string delivery = GetOption(args, "--delivery") ?? "package";
        string author = GetOption(args, "--author") ?? Environment.UserName;

        try
        {
            new PluginScaffoldGenerator(pluginId, Directory.GetCurrentDirectory(), category, delivery, author).Generate();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static string? GetOption(string[] args, string option)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i].Equals(option, StringComparison.OrdinalIgnoreCase))
            {
                return args[i + 1];
            }
        }

        return null;
    }
}

// <copyright file="NewCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;

/// <summary>
/// Olav new project cli command.
/// </summary>
public static class NewCommand
{
    /// <summary>
    /// Executes new command to generate a clean new project.
    /// </summary>
    /// <param name="args">Arguments to the new project.</param>
    public static void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Project name is required.");
            return;
        }

        string name = args[1];
        string owner = GetOption(args, "--owner") ?? Environment.UserName;
        string license = GetOption(args, "--license") ?? "MIT";

        string root = Path.Combine(Directory.GetCurrentDirectory(), name);

        if (Directory.Exists(root))
        {
            Console.WriteLine("Directory already exists.");
            return;
        }

        Console.WriteLine($"Creating Olav project: {name}");

        new BaseStructureGenerator(root).Generate();
        new VersionEnforcementGenerator(root).Generate();
        new ProjectGenerator(name, root).Generate();
        new TestGenerator(name, root).Generate();
        new SolutionGenerator(name, root).Generate();
        new FileTemplateGenerator(name, root, owner, license).Generate();
        new ObservabilityGenerator(name, root, owner, license).Generate();
        new GitGenerator(root).Generate();

        Console.WriteLine("Olav solution created successfully.");
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

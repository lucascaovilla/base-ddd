// <copyright file="NewCommand.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Commands;

using BaseDDD.Generation;
using BaseDDD.Infrastructure;

/// <summary>
/// BaseDDD new project cli command.
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

        Console.WriteLine($"Creating BaseDDD project: {name}");

        CreateBaseStructure(root);

        // Solution
        new ProjectGenerator(name, root).Generate();
        new TestGenerator(name, root).Generate();
        new SolutionGenerator(name, root).Generate();
        new FileTemplateGenerator(name, root, owner, license).Generate();
        new ObservabilityGenerator(name, root, owner, license).Generate();

        InitializeGit(root);

        Console.WriteLine("BaseDDD solution created successfully.");
    }

    private static void CreateBaseStructure(string root)
    {
        FileSystem.CreateDirectory(root);
        FileSystem.CreateDirectory(Path.Combine(root, "src"));
        FileSystem.CreateDirectory(Path.Combine(root, "tests"));
        FileSystem.CreateDirectory(Path.Combine(root, "docker"));
    }

    private static void InitializeGit(string root)
    {
        GitRunner.Run("init", root);
        GitRunner.Run("branch -m main", root);
        GitRunner.Run("config core.hooksPath .githooks", root);
        GitRunner.Run("add .", root);
        GitRunner.Run("commit -m \"Initial BaseDDD structure\"", root);
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

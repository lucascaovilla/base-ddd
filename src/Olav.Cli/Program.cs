// <copyright file="Program.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Runtime.CompilerServices;
using Olav.Commands;

[assembly: InternalsVisibleTo("Olav.IntegrationTests")]
[assembly: InternalsVisibleTo("Olav.ArchitectureTests")]
[assembly: InternalsVisibleTo("Olav.UnitTests")]

namespace Olav;

/// <summary>
/// Entry point for the Olav CLI.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    internal static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintHelp();
            return;
        }

        string command = args[0].ToLowerInvariant();

        switch (command)
        {
            case "new":
                NewCommand.Execute(args);
                break;

            case "lint":
                LintCommand.Execute();
                break;

            case "verify":
                VerifyCommand.Execute();
                break;
            case "migrate":
                MigrateCommand.Execute(args);
                break;
            default:
                PrintHelp();
                break;
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine(
        """
        Olav CLI

        Usage:
          olav new <ProjectName>
          olav lint
          olav verify
        """);
    }
}

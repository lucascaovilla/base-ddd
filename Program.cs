// <copyright file="Program.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace BaseDDD;

using System;
using BaseDDD.Commands;

/// <summary>
/// Entry point for the BaseDDD CLI.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    private static void Main(string[] args)
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

            default:
                PrintHelp();
                break;
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine(
        """
        BaseDDD CLI

        Usage:
          baseddd new <ProjectName>
          baseddd lint
          baseddd verify
        """);
    }
}

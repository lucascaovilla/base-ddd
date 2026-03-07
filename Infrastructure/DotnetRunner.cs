// <copyright file="DotnetRunner.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Infrastructure;

using System.Diagnostics;

/// <summary>
/// Dotnet commands runner.
/// </summary>
public static class DotnetRunner
{
    /// <summary>
    /// Runs dotnet commands.
    /// </summary>
    /// <param name="args">Arguments to the command.</param>
    /// <param name="workingDirectory">Reference directory to run.</param>
    public static void Run(string args, string workingDirectory)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            },
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"dotnet {args} failed.");
        }
    }
}

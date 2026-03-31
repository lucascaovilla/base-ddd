// <copyright file="DotnetRunner.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

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
        Process process = new Process()
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

        string stdout = process.StandardOutput.ReadToEnd();
        string stderr = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception(
                $"dotnet {args} failed.\n\nSTDOUT:\n{stdout}\n\nSTDERR:\n{stderr}");
        }
    }
}

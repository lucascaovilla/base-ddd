// <copyright file="GitRunner.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

using System.Diagnostics;

/// <summary>
/// Git commands runner.
/// </summary>
public static class GitRunner
{
    /// <summary>
    /// Runs git commands.
    /// </summary>
    /// <param name="args">Arguments to the command.</param>
    /// <param name="workingDirectory">Reference directory to run.</param>
    public static void Run(string args, string workingDirectory)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
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
            if (args.StartsWith("commit") &&
                (stdout.Contains("nothing to commit") || stdout.Contains("nothing added to commit")))
            {
                return;
            }

            throw new Exception($"git {args} failed.\n\nSTDOUT:\n{stdout}\n\nSTDERR:\n{stderr}");
        }
    }
}

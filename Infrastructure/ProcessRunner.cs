// <copyright file="ProcessRunner.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Infrastructure;

using System.Diagnostics;

/// <summary>
/// Runner for generic system commands.
/// </summary>
public static class ProcessRunner
{
    /// <summary>
    /// Runs generic system commands.
    /// </summary>
    /// <param name="fileName">Name of the command file to be run.</param>
    /// <param name="arguments">Arguments to the command.</param>
    /// <param name="workingDirectory">Reference directory to run.</param>
    public static void Run(string fileName, string arguments, string workingDirectory)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
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
            throw new Exception($"{fileName} {arguments} failed.");
        }
    }
}

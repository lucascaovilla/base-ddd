// <copyright file="ProcessRunner.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

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
    /// <returns>Returns command output.</returns>
    public static string Run(string fileName, string arguments, string workingDirectory)
    {
        using Process process = new Process()
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
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Command failed: {fileName} {arguments}\nSTDOUT:\n{output}\nSTDERR:\n{error}");
        }

        return output;
    }
}

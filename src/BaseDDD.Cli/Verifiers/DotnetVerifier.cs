// <copyright file="DotnetVerifier.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace BaseDDD.Verifiers;

using BaseDDD.Infrastructure;

/// <summary>
/// Generic verifier for dotnet structure.
/// </summary>
public static class DotnetVerifier
{
    /// <summary>
    /// Runs dotnet commands .
    /// </summary>
    /// <param name="root">Root reference folder.</param>
    /// <param name="command">Dotnet command to be verified.</param>
    public static void VerifyCommand(string root, string command)
    {
        DotnetRunner.Run(command, root);
    }
}

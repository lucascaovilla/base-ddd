// <copyright file="VerifyCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Helpers;
using Olav.Verifiers;

/// <summary>
/// Olav verify project structure command.
/// </summary>
public static class VerifyCommand
{
    /// <summary>
    /// Executes verify on project via cli tool.
    /// </summary>
    public static void Execute()
    {
        LintCommand.Execute();

        string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());

        DotnetVerifier.VerifyCommand(root, "build");
        DotnetVerifier.VerifyCommand(root, "test");

        Console.WriteLine("Project full verification passed!");
    }
}

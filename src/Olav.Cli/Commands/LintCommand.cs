// <copyright file="LintCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;
using Olav.Helpers;
using Olav.Verifiers;

/// <summary>
/// Olav lint project cli command.
/// </summary>
public static class LintCommand
{
    /// <summary>
    /// Executes lint on project via cli tool.
    /// </summary>
    public static void Execute()
    {
        string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());

        new VersionEnforcementGenerator(root).Check();

        ProjectVerifier.VerifyDirectory(root, "src");

        ProjectVerifier.VerifyDirectory(root, "tests");
        ProjectVerifier.VerifyDirectory(root, "docker");

        ProjectVerifier.VerifyFile(root, "Directory.Build.props");
        ProjectVerifier.VerifyFile(root, "stylecop.json");

        Console.WriteLine("Basic project structure verification passed!");
    }
}

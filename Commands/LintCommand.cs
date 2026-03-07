// <copyright file="LintCommand.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Commands;

using BaseDDD.Infrastructure;

/// <summary>
/// BaseDDD lint project cli command.
/// </summary>
public static class LintCommand
{
    /// <summary>
    /// Executes lint on project via cli tool.
    /// </summary>
    public static void Execute()
    {
        string root = Directory.GetCurrentDirectory();

        FileSystem.ValidateDirectory(root, "src");
        FileSystem.ValidateDirectory(root, "tests");
        FileSystem.ValidateDirectory(root, "docker");

        FileSystem.ValidateFile(root, "Directory.Build.props");
        FileSystem.ValidateFile(root, "stylecop.json");
    }
}

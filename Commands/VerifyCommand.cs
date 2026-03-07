// <copyright file="VerifyCommand.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Commands;

using BaseDDD.Infrastructure;

/// <summary>
/// BaseDDD verify project structure command.
/// </summary>
public static class VerifyCommand
{
    /// <summary>
    /// Executes verify on project via cli tool.
    /// </summary>
    public static void Execute()
    {
        LintCommand.Execute();

        string root = Directory.GetCurrentDirectory();

        DotnetRunner.Run("build", root);
        DotnetRunner.Run("test", root);
    }
}

// <copyright file="PreCommitTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides pre-commit file template.
/// </summary>
public static class PreCommitTemplate
{
    /// <summary>
    /// Returns content of generated pre-commit.
    /// </summary>
    /// <returns>pre-commit file content.</returns>
    public static string Generate()
    {
        return $"""
        #!/bin/sh

        {CheckMissingToolTemplate.Generate()}

        echo "Running pre-commit checks..."

        baseddd lint
        if [ $? -ne 0 ]; then
          echo "Lint failed"
          exit 1
        fi
        """;
    }
}

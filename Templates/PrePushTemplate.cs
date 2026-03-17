// <copyright file="PrePushTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides pre-push file template.
/// </summary>
public static class PrePushTemplate
{
    /// <summary>
    /// Returns content of generated pre-push.
    /// </summary>
    /// <returns>pre-push file content.</returns>
    public static string Generate()
    {
        return $"""
        #!/bin/sh

        {CheckMissingToolTemplate.Generate()}

        echo "Running pre-push checks..."

        baseddd verify
        if [ $? -ne 0 ]; then
        echo "Verification failed"
        exit 1
        fi
        """;
    }
}

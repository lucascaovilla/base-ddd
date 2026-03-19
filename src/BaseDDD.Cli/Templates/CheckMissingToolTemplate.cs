// <copyright file="CheckMissingToolTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides CheckMissingTool script template.
/// </summary>
public static class CheckMissingToolTemplate
{
    /// <summary>
    /// Returns content of generated CheckMissingTool script.
    /// </summary>
    /// <returns>CheckMissingTool script content.</returns>
    public static string Generate()
    {
        return """
        echo "Running CheckMissingTool script checks..."

        baseddd lint
        if [ $? -ne 0 ]; then
        echo "Verification failed"
        exit 1
        fi
        """;
    }
}

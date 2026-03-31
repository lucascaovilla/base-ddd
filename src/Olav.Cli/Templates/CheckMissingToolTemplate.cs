// <copyright file="CheckMissingToolTemplate.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Templates;

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

        olav lint
        if [ $? -ne 0 ]; then
        echo "Verification failed"
        exit 1
        fi
        """;
    }
}

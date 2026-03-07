// <copyright file="GlobalJsonTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides global.json.cs file template.
/// </summary>
public static class GlobalJsonTemplate
{
    /// <summary>
    /// Returns content of generated global.json.
    /// </summary>
    /// <returns>global.json file content.</returns>
    public static string Generate()
    {
        return """
        {
            "sdk": {
            "version": "10.0.100",
            "rollForward": "latestFeature"
            }
        }
        """;
    }
}

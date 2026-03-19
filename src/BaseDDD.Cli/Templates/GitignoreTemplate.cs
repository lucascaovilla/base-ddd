// <copyright file="GitignoreTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides .gitignore file template.
/// </summary>
public static class GitignoreTemplate
{
    /// <summary>
    /// Returns content of generated .gitignore.
    /// </summary>
    /// <returns>.gitignore file content.</returns>
    public static string Generate()
    {
        return """
        .env
        **/nupkg
        **/bin
        **/obj
        publish
        **/TestResults/
        """;
    }
}

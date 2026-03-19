// <copyright file="DockerignoreTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides .dockerignore file template.
/// </summary>
public static class DockerignoreTemplate
{
    /// <summary>
    /// Returns content of generated .dockerignore.
    /// </summary>
    /// <returns>.dockerignore file content.</returns>
    public static string Generate()
    {
        return """
        bin/
        obj/
        .git/
        .gitignore
        Dockerfile*
        docker-compose*
        node_modules/
        *.md
        """;
    }
}

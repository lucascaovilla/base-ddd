// <copyright file="DockerComposeTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides docker-compose files templates.
/// </summary>
public static class DockerComposeTemplate
{
    /// <summary>
    /// Returns content of generated production docker compose.
    /// </summary>
    /// <param name="name">Repository name.</param>
    /// <returns>Content of production docker compose.</returns>
    public static string GeneratePrd(string name)
    {
        return $"""
        services:
            api:
                image: {name.ToLowerInvariant()}-api:latest
                build:
                    context: ..
                    dockerfile: docker/Dockerfile
                ports:
                    - "8080:8080"
                environment:
                    - ASPNETCORE_ENVIRONMENT=Production
        """;
    }

    /// <summary>
    /// Returns content of generated staging docker compose.
    /// </summary>
    /// <param name="name">Repository name.</param>
    /// <returns>Content of staging docker compose.</returns>
    public static string GenerateStaging(string name)
    {
        return $"""
        services:
            api:
                image: {name.ToLowerInvariant()}-api:staging
                build:
                    context: ..
                    dockerfile: docker/Dockerfile
                ports:
                    - "8081:8080"
                environment:
                    - ASPNETCORE_ENVIRONMENT=Staging
        """;
    }

    /// <summary>
    /// Returns content of generated development docker compose.
    /// </summary>
    /// <param name="name">Repository name.</param>
    /// <returns>Content of development docker compose.</returns>
    public static string GenerateDev(string name)
    {
        return $"""
        services:
            api:
                build:
                    context: ..
                    dockerfile: docker/Dockerfile
                volumes:
                    - ..:/src
                working_dir: /src
                command: dotnet watch run --project src/{name.ToLowerInvariant()}.Api
                ports:
                    - "8082:8080"
                environment:
                    - ASPNETCORE_ENVIRONMENT=Development
        """;
    }

    /// <summary>
    /// Returns content of generated local docker compose.
    /// </summary>
    /// <param name="name">Repository name.</param>
    /// <returns>Content of local docker compose.</returns>
    public static string GenerateLocal(string name)
    {
        return $"""
        services:
            api:
                build:
                    context: ..
                    dockerfile: docker/Dockerfile
                ports:
                    - "8080:8080"
                environment:
                    - ASPNETCORE_ENVIRONMENT=Development
                    - ConnectionStrings__Default=Host=postgres;Port=5432;Database={name.ToLowerInvariant()};Username=postgres;Password=postgres
                depends_on:
                    - postgres

            postgres:
                image: postgres:16
                restart: always
                environment:
                    POSTGRES_DB: {name.ToLowerInvariant()}
                    POSTGRES_USER: postgres
                    POSTGRES_PASSWORD: postgres
                ports:
                    - "5432:5432"
        """;
    }
}

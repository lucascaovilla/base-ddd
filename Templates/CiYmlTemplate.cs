// <copyright file="CiYmlTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides ci.yml file template.
/// </summary>
public static class CiYmlTemplate
{
    /// <summary>
    /// Returns content of generated ci.yml.
    /// </summary>
    /// <returns>ci.yml file content.</returns>
    public static string Generate()
    {
        return """
        name: CI

        on:
            push:
            branches: [ main ]
            pull_request:

        jobs:
            build:
            runs-on: ubuntu-latest

            steps:
                - uses: actions/checkout@v4

                - uses: actions/setup-dotnet@v4
                with:
                    dotnet-version: '10.0.x'

                - name: Restore
                run: dotnet restore

                - name: Build
                run: dotnet build --no-restore

                - name: Test
                run: dotnet test --no-build
        """;
    }
}

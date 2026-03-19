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
        name: Architecture Verification

        on:
          push:
            branches:
              - "**"
          pull_request:
            branches:
              - main

        jobs:
          verify:
              runs-on: ubuntu-latest

              steps:
              - name: Checkout repository
                uses: actions/checkout@v4

              - name: Setup .NET
                uses: actions/setup-dotnet@v4
                with:
                  dotnet-version: "10.0.x"

              - name: Install BaseDDD CLI
                run: dotnet tool install -g BaseDDD.Cli

              - name: Restore dependencies
                run: dotnet restore

              - name: Run BaseDDD verification
                run: baseddd verify
        """;
    }
}

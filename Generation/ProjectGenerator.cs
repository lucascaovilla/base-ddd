// <copyright file="ProjectGenerator.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Generation;

using BaseDDD.Infrastructure;

/// <summary>
/// Generates BaseDDD project.
/// </summary>
public class ProjectGenerator
{
    private readonly string name;
    private readonly string root;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectGenerator"/> class.
    /// </summary>
    /// <param name="name">Project's name.</param>
    /// <param name="root">Repository's root.</param>
    public ProjectGenerator(string name, string root)
    {
        this.name = name;
        this.root = root;
    }

    /// <summary>
    /// Centralized Generate method to create project's layers and files.
    /// </summary>
    public void Generate()
    {
        string src = Path.Combine(this.root, "src");

        DotnetRunner.Run($"new classlib -n {this.name}.Domain -f net10.0", src);
        DotnetRunner.Run($"new classlib -n {this.name}.Application -f net10.0", src);
        DotnetRunner.Run($"new classlib -n {this.name}.Infrastructure -f net10.0", src);
        DotnetRunner.Run($"new webapi -n {this.name}.Web -f net10.0 --no-https", src);

        this.AddPackages();
    }

    /// <summary>
    /// Adds references between generated layers.
    /// </summary>
    private void AddProjectReferences()
    {
        DotnetRunner.Run($"add src/{this.name}.Application/{this.name}.Application.csproj reference src/{this.name}.Domain/{this.name}.Domain.csproj", this.root);

        DotnetRunner.Run($"add src/{this.name}.Infrastructure/{this.name}.Infrastructure.csproj reference src/{this.name}.Application/{this.name}.Application.csproj", this.root);
        DotnetRunner.Run($"add src/{this.name}.Infrastructure/{this.name}.Infrastructure.csproj reference src/{this.name}.Domain/{this.name}.Domain.csproj", this.root);

        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj reference src/{this.name}.Application/{this.name}.Application.csproj", this.root);
        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj reference src/{this.name}.Infrastructure/{this.name}.Infrastructure.csproj", this.root);

        DotnetRunner.Run($"add tests/{this.name}.ArchitectureTests/{this.name}.ArchitectureTests.csproj reference src/{this.name}.Application/{this.name}.Application.csproj", this.root);
        DotnetRunner.Run($"add tests/{this.name}.ArchitectureTests/{this.name}.ArchitectureTests.csproj reference src/{this.name}.Domain/{this.name}.Domain.csproj", this.root);

        DotnetRunner.Run($"add tests/{this.name}.IntegrationTests/{this.name}.IntegrationTests.csproj reference src/{this.name}.Web/{this.name}.Web.csproj", this.root);
    }

    /// <summary>
    /// Adds required packages to generated layers.
    /// </summary>
    private void AddPackages()
    {
        DotnetRunner.Run($"add src/{this.name}.Domain/{this.name}.Domain.csproj package StyleCop.Analyzers", this.root);
        DotnetRunner.Run($"add src/{this.name}.Application/{this.name}.Application.csproj package StyleCop.Analyzers", this.root);
        DotnetRunner.Run($"add src/{this.name}.Infrastructure/{this.name}.Infrastructure.csproj package StyleCop.Analyzers", this.root);

        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj package StyleCop.Analyzers", this.root);
        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj package Serilog.AspNetCore", this.root);
        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj package Serilog.Sinks.Console", this.root);

        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj package OpenTelemetry.Extensions.Hosting", this.root);
        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj package OpenTelemetry.Instrumentation.AspNetCore", this.root);
        DotnetRunner.Run($"add src/{this.name}.Web/{this.name}.Web.csproj package OpenTelemetry.Exporter.Console", this.root);
    }
}

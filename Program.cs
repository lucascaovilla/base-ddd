// <copyright file="Program.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace BaseDDD.Cli;

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

/// <summary>
/// Entry point for the BaseDDD CLI.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintHelp();
            return;
        }

        string command = args[0].ToLowerInvariant();

        switch (command)
        {
            case "new":
                HandleNew(args);
                break;

            case "lint":
                HandleLint();
                break;

            case "verify":
                HandleVerify();
                break;

            default:
                Console.WriteLine($"Unknown command: {command}");
                PrintHelp();
                break;
        }
    }

    /// <summary>
    /// Handles the 'new' command.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    private static void HandleNew(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Project name is required.");
            return;
        }

        string name = args[1];
        string rootPath = Path.Combine(Directory.GetCurrentDirectory(), name);

        if (Directory.Exists(rootPath))
        {
            Console.WriteLine("Directory already exists.");
            return;
        }

        string editorConfig = """
        root = true

        [*.cs]
        dotnet_style_require_accessibility_modifiers = always:error
        dotnet_style_qualification_for_field = true:error
        csharp_style_var_for_built_in_types = false:error
        csharp_style_var_when_type_is_apparent = false:error
        csharp_style_var_elsewhere = false:error
        csharp_style_expression_bodied_methods = false:error
        csharp_style_namespace_declarations = file_scoped:error
        dotnet_diagnostic.IDE0051.severity = error
        dotnet_diagnostic.CA1822.severity = error
        """;
        WriteFile(Path.Combine(rootPath, ".editorconfig"), editorConfig);

        string directoryBuildProps = """
        <Project>
            <PropertyGroup>
            <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
            <Nullable>enable</Nullable>
            <ImplicitUsings>enable</ImplicitUsings>
            <AnalysisLevel>latest</AnalysisLevel>
            <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
            </PropertyGroup>
        </Project>
        """;
        WriteFile(Path.Combine(rootPath, "Directory.Build.props"), directoryBuildProps);

        string directoryPackagesProps = """
        <Project>
            <ItemGroup>
            <PackageVersion Include="StyleCop.Analyzers" Version="1.2.0-beta.507" />
            <PackageVersion Include="coverlet.collector" Version="6.0.0" />
            </ItemGroup>
        </Project>
        """;
        WriteFile(Path.Combine(rootPath, "Directory.Packages.props"), directoryPackagesProps);

        string globalJson = """
        {
            "sdk": {
            "version": "10.0.100",
            "rollForward": "latestFeature"
            }
        }
        """;
        WriteFile(
            Path.Combine(rootPath, "global.json"), globalJson);

        string ciYml = """
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
        WriteFile(Path.Combine(rootPath, ".github/workflows/ci.yml"), ciYml);

        Console.WriteLine($"Creating BaseDDD project: {name}");

        Directory.CreateDirectory(rootPath);
        Directory.CreateDirectory(Path.Combine(rootPath, "src"));
        Directory.CreateDirectory(Path.Combine(rootPath, "tests"));
        Directory.CreateDirectory(Path.Combine(rootPath, "docker"));

        RunDotnet($"new sln -n {name}", rootPath);

        string srcPath = Path.Combine(rootPath, "src");
        string testsPath = Path.Combine(rootPath, "tests");

        RunDotnet($"new classlib -n {name}.Domain -f net10.0", srcPath);
        RunDotnet($"new classlib -n {name}.Application -f net10.0", srcPath);
        RunDotnet($"new classlib -n {name}.Infrastructure -f net10.0", srcPath);
        RunDotnet($"new webapi -n {name}.Web -f net10.0 --no-https", srcPath);

        RunDotnet($"new xunit -n {name}.ArchitectureTests -f net10.0", testsPath);
        RunDotnet($"new xunit -n {name}.IntegrationTests -f net10.0", testsPath);

        RunDotnet($"sln add src/{name}.Domain/{name}.Domain.csproj", rootPath);
        RunDotnet($"sln add src/{name}.Application/{name}.Application.csproj", rootPath);
        RunDotnet($"sln add src/{name}.Infrastructure/{name}.Infrastructure.csproj", rootPath);
        RunDotnet($"sln add src/{name}.Web/{name}.Web.csproj", rootPath);

        RunDotnet($"add src/{name}.Domain/{name}.Domain.csproj package StyleCop.Analyzers", rootPath);
        RunDotnet($"add src/{name}.Application/{name}.Application.csproj package StyleCop.Analyzers", rootPath);
        RunDotnet($"add src/{name}.Infrastructure/{name}.Infrastructure.csproj package StyleCop.Analyzers", rootPath);
        RunDotnet($"add src/{name}.Web/{name}.Web.csproj package StyleCop.Analyzers", rootPath);
        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj package StyleCop.Analyzers", rootPath);
        RunDotnet($"add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj package StyleCop.Analyzers", rootPath);

        RunDotnet($"sln add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj", rootPath);
        RunDotnet($"sln add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj", rootPath);

        RunDotnet(
            $"add src/{name}.Application/{name}.Application.csproj reference src/{name}.Domain/{name}.Domain.csproj",
            rootPath);

        RunDotnet(
            $"add src/{name}.Infrastructure/{name}.Infrastructure.csproj reference src/{name}.Application/{name}.Application.csproj",
            rootPath);

        RunDotnet(
            $"add src/{name}.Infrastructure/{name}.Infrastructure.csproj reference src/{name}.Domain/{name}.Domain.csproj",
            rootPath);

        RunDotnet(
            $"add src/{name}.Web/{name}.Web.csproj reference src/{name}.Application/{name}.Application.csproj",
            rootPath);

        RunDotnet(
            $"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Domain/{name}.Domain.csproj",
            rootPath);

        RunDotnet(
            $"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Application/{name}.Application.csproj",
            rootPath);

        RunDotnet(
            $"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Infrastructure/{name}.Infrastructure.csproj",
            rootPath);

        RunDotnet(
            $"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Web/{name}.Web.csproj",
            rootPath);

        RunDotnet(
            $"add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj reference src/{name}.Web/{name}.Web.csproj",
            rootPath);

        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj package coverlet.collector", rootPath);
        RunDotnet($"add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj package coverlet.collector", rootPath);

        string archTestProj = Path.Combine(rootPath, $"tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj");
        string integrationTestProj = Path.Combine(rootPath, $"tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj");

        AddCoverageEnforcement(archTestProj);
        AddCoverageEnforcement(integrationTestProj);

        string initialTest = $$"""
        using Xunit;

        namespace {{name}}.ArchitectureTests;

        public class InitialTests
        {
            [Fact]
            public void Should_Pass()
            {
                Assert.True(true);
            }
        }
        """;
        WriteFile(Path.Combine(rootPath, $"tests/{name}.ArchitectureTests/InitialTests.cs"), initialTest);

        DeleteIfExists(Path.Combine(srcPath, $"{name}.Domain/Class1.cs"));
        DeleteIfExists(Path.Combine(srcPath, $"{name}.Application/Class1.cs"));
        DeleteIfExists(Path.Combine(srcPath, $"{name}.Infrastructure/Class1.cs"));

        DeleteIfExists(Path.Combine(testsPath, $"{name}.ArchitectureTests/UnitTest1.cs"));
        DeleteIfExists(Path.Combine(testsPath, $"{name}.IntegrationTests/UnitTest1.cs"));

        DeleteIfExists(Path.Combine(srcPath, $"{name}.Web/Controllers/WeatherForecastController.cs"));
        DeleteIfExists(Path.Combine(srcPath, $"{name}.Web/WeatherForecast.cs"));

        RunGit("init", rootPath);
        RunGit("config core.hooksPath .githooks", rootPath);
        RunGit("add .", rootPath);
        RunGit("commit -m \"Initial BaseDDD structure\"", rootPath);

        Console.WriteLine("BaseDDD solution created successfully.");
    }

    /// <summary>
    /// Handles the 'lint' command.
    /// </summary>
    private static void HandleLint()
    {
        Console.WriteLine("Running structure lint...");
    }

    /// <summary>
    /// Handles the 'verify' command.
    /// </summary>
    private static void HandleVerify()
    {
        Console.WriteLine("Verifying architecture...");
    }

    /// <summary>
    /// Prints CLI help information.
    /// </summary>
    private static void PrintHelp()
    {
        Console.WriteLine(
            """
            BaseDDD CLI

            Usage:
              baseddd new <ProjectName>
              baseddd lint
              baseddd verify
            """);
    }

    /// <summary>
    /// Runs a dotnet CLI command.
    /// </summary>
    /// <param name="arguments">Arguments to pass to dotnet.</param>
    /// <param name="workingDirectory">Working directory.</param>
    private static void RunDotnet(string arguments, string workingDirectory)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            },
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"dotnet {arguments} failed.");
        }
    }

    /// <summary>
    /// Deletes a file if it exists.
    /// </summary>
    /// <param name="path">File path.</param>
    private static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Writes a given file to the given directory.
    /// </summary>
    /// <param name="path">File path.</param>
    /// <param name="content">File content.</param>
    private static void WriteFile(string path, string content)
    {
        string? directory = Path.GetDirectoryName(path);

        if (directory is null)
        {
            throw new InvalidOperationException("Invalid file path.");
        }

        Directory.CreateDirectory(directory);
        File.WriteAllText(path, content);
    }

    /// <summary>
    /// Adds coverage enforcement to .csproj files.
    /// </summary>
    /// <param name="csprojPath">Csproj file path.</param>
    private static void AddCoverageEnforcement(string csprojPath)
    {
        XDocument doc = XDocument.Load(csprojPath);

        XElement? project = doc.Element("Project");
        if (project == null)
        {
            throw new Exception("Invalid csproj format.");
        }

        XElement propertyGroup = new XElement(
            "PropertyGroup",
            new XElement("CollectCoverage", "true"),
            new XElement("CoverletOutputFormat", "lcov"),
            new XElement("Threshold", "100"),
            new XElement("ThresholdType", "line"),
            new XElement("ThresholdStat", "Total"));

        project.Add(propertyGroup);

        doc.Save(csprojPath);
    }

    /// <summary>
    /// Runs git commands.
    /// </summary>
    /// <param name="arguments">Git arguments to run.</param>
    /// <param name="workingDirectory">Directory to apply the commands.</param>
    private static void RunGit(string arguments, string workingDirectory)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            },
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"git {arguments} failed.");
        }
    }
}

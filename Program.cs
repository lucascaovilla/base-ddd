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
        string owner = GetOption(args, "--owner") ?? Environment.UserName;
        string license = GetOption(args, "--license") ?? "MIT";

        string rootPath = Path.Combine(Directory.GetCurrentDirectory(), name);

        if (Directory.Exists(rootPath))
        {
            Console.WriteLine("Directory already exists.");
            return;
        }

        WriteFile(Path.Combine(rootPath, ".editorconfig"), GetEditorConfigContent());
        WriteFile(Path.Combine(rootPath, "stylecop.json"), GetStyleCopJsonContent(owner, license));
        WriteFile(Path.Combine(rootPath, "LICENSE"), GetLicenseContent(owner, license));
        WriteFile(Path.Combine(rootPath, "Directory.Build.props"), GetDirectoryBuildPropsContent());
        WriteFile(Path.Combine(rootPath, "Directory.Packages.props"), GetDirectoryPackagePropsContent());
        WriteFile(Path.Combine(rootPath, "global.json"), GetGlobalJsonContent());

        WriteFile(Path.Combine(rootPath, ".github/workflows/ci.yml"), GetCiYmlContent());

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
        RunDotnet($"add src/{name}.Web/{name}.Web.csproj package Serilog.AspNetCore", rootPath);
        RunDotnet($"add src/{name}.Web/{name}.Web.csproj package Serilog.Sinks.Console", rootPath);
        RunDotnet($"add src/{name}.Web/{name}.Web.csproj package OpenTelemetry.Extensions.Hosting", rootPath);
        RunDotnet($"add src/{name}.Web/{name}.Web.csproj package OpenTelemetry.Instrumentation.AspNetCore", rootPath);
        RunDotnet($"add src/{name}.Web/{name}.Web.csproj package OpenTelemetry.Exporter.Console", rootPath);

        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj package StyleCop.Analyzers", rootPath);
        RunDotnet($"add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj package StyleCop.Analyzers", rootPath);

        RunDotnet($"sln add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj", rootPath);
        RunDotnet($"sln add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj", rootPath);

        RunDotnet($"add src/{name}.Application/{name}.Application.csproj reference src/{name}.Domain/{name}.Domain.csproj", rootPath);
        RunDotnet($"add src/{name}.Infrastructure/{name}.Infrastructure.csproj reference src/{name}.Application/{name}.Application.csproj", rootPath);
        RunDotnet($"add src/{name}.Infrastructure/{name}.Infrastructure.csproj reference src/{name}.Domain/{name}.Domain.csproj", rootPath);
        RunDotnet($"add src/{name}.Web/{name}.Web.csproj reference src/{name}.Application/{name}.Application.csproj", rootPath);
        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Domain/{name}.Domain.csproj", rootPath);
        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Application/{name}.Application.csproj", rootPath);
        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Infrastructure/{name}.Infrastructure.csproj", rootPath);
        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Web/{name}.Web.csproj", rootPath);
        RunDotnet($"add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj reference src/{name}.Web/{name}.Web.csproj", rootPath);

        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj package coverlet.collector", rootPath);
        RunDotnet($"add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj package coverlet.collector", rootPath);

        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj package NetArchTest.Rules", rootPath);
        RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj package FluentAssertions", rootPath);

        string archTestProj = Path.Combine(rootPath, $"tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj");
        string integrationTestProj = Path.Combine(rootPath, $"tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj");

        AddCoverageEnforcement(archTestProj);
        AddCoverageEnforcement(integrationTestProj);

        string archTestsPath = Path.Combine(rootPath, $"tests/{name}.ArchitectureTests");

        WriteFile(Path.Combine(archTestsPath, "DependencyRulesTests.cs"), GetDependencyRulesTestsContent(name, owner, license));
        WriteFile(Path.Combine(archTestsPath, "ObservabilityRulesTests.cs"), GetObservabilityRulesTestsContent(name, owner, license));

        string integrationTestsPath = Path.Combine(rootPath, $"tests/{name}.IntegrationTests");

        WriteFile(Path.Combine(integrationTestsPath, "InitialIntegrationTests.cs"), GetInitialIntegrationTestContent(name, owner, license));

        DeleteIfExists(Path.Combine(srcPath, $"{name}.Domain/Class1.cs"));
        DeleteIfExists(Path.Combine(srcPath, $"{name}.Application/Class1.cs"));
        DeleteIfExists(Path.Combine(srcPath, $"{name}.Infrastructure/Class1.cs"));

        DeleteIfExists(Path.Combine(testsPath, $"{name}.ArchitectureTests/UnitTest1.cs"));
        DeleteIfExists(Path.Combine(testsPath, $"{name}.IntegrationTests/UnitTest1.cs"));

        DeleteIfExists(Path.Combine(srcPath, $"{name}.Web/Controllers/WeatherForecastController.cs"));
        DeleteIfExists(Path.Combine(srcPath, $"{name}.Web/WeatherForecast.cs"));

        string webPath = Path.Combine(srcPath, $"{name}.Web");
        string observabilityPath = Path.Combine(webPath, "Observability");

        Directory.CreateDirectory(observabilityPath);

        WriteFile(Path.Combine(observabilityPath, "CorrelationMiddleware.cs"), GetCorrelationMiddlewareContent(name, owner, license));
        WriteFile(Path.Combine(observabilityPath, "ObservabilityExtensions.cs"), GetObservabilityExtensionsContent(name, owner, license));

        WriteFile(Path.Combine(webPath, "Program.cs"), GetProgramFileContent(name, owner, license));

        RunGit("init", rootPath);
        RunGit("config core.hooksPath .githooks", rootPath);
        RunGit("add .", rootPath);
        RunGit("commit -m \"Initial BaseDDD structure\"", rootPath);

        Console.WriteLine("BaseDDD solution created successfully.");
    }

    /// <summary>
    /// Returns the value of the option passed in args.
    /// </summary>
    private static string? GetOption(string[] args, string option)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i].Equals(option, StringComparison.OrdinalIgnoreCase))
            {
                return args[i + 1];
            }
        }

        return null;
    }

    /// <summary>
    /// Returns content of generated .editorconfig.
    /// </summary>
    private static string GetEditorConfigContent()
    {
        return """
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
    }

    /// <summary>
    /// Returns content of generated stylecop.json.
    /// </summary>
    private static string GetStyleCopJsonContent(string owner, string license)
    {
        int year = DateTime.UtcNow.Year;

        return $$"""
        {
            "settings": {
                "documentationRules": {
                    "companyName": "{{owner}}",
                    "copyrightText": "Copyright (c) {{year}} {{owner}}. Licensed under the {{license}} License."
                }
            }
        }
        """;
    }

    /// <summary>
    /// Generates the LICENSE file content.
    /// </summary>
    private static string GetLicenseContent(string owner, string license)
    {
        int year = DateTime.UtcNow.Year;

        if (license.Equals("MIT", StringComparison.OrdinalIgnoreCase))
        {
            return $$"""
            MIT License

            Copyright (c) {{year}} {{owner}}

            Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

            The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

            THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
            """;
        }

        return $"License '{license}' not yet supported.";
    }

    /// <summary>
    /// Returns content of generated Directory.Build.props.
    /// </summary>
    private static string GetDirectoryBuildPropsContent()
    {
        return """
        <Project>
            <PropertyGroup>
                <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
                <Nullable>enable</Nullable>
                <ImplicitUsings>enable</ImplicitUsings>
                <AnalysisLevel>latest</AnalysisLevel>
                <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
                <GenerateDocumentationFile>true</GenerateDocumentationFile>
                <NoWarn>$(NoWarn);1591</NoWarn>
            </PropertyGroup>

            <ItemGroup>
                <AdditionalFiles Include="$(MSBuildThisFileDirectory)stylecop.json" />
            </ItemGroup>
        </Project>
        """;
    }

    /// <summary>
    /// Returns content of generated Directory.Package.props.
    /// </summary>
    private static string GetDirectoryPackagePropsContent()
    {
        return """
        <Project>
            <ItemGroup>
            <PackageVersion Include="StyleCop.Analyzers" Version="1.2.0-beta.507" />
            <PackageVersion Include="coverlet.collector" Version="6.0.0" />
            </ItemGroup>
        </Project>
        """;
    }

    /// <summary>
    /// Returns content of generated global.json.
    /// </summary>
    private static string GetGlobalJsonContent()
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

    /// <summary>
    /// Returns content of generated ci.yml.
    /// </summary>
    private static string GetCiYmlContent()
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

    /// <summary>
    /// Returns content of generated dependency rules test.
    /// </summary>
    private static string GetDependencyRulesTestsContent(string name, string owner, string license)
    {
        return GetFileHeader("DependencyRulesTests.cs", owner, license) + $$"""
        namespace {{name}}.ArchitectureTests;

        using System.Reflection;
        using NetArchTest.Rules;
        using Xunit;

        /// <summary>
        /// Enforces Clean Architecture dependency rules.
        /// </summary>
        public sealed class DependencyRulesTests
        {
            private static readonly Assembly DomainAssembly =
                Assembly.Load("{{name}}.Domain");

            private static readonly Assembly ApplicationAssembly =
                Assembly.Load("{{name}}.Application");

            private static readonly Assembly InfrastructureAssembly =
                Assembly.Load("{{name}}.Infrastructure");

            /// <summary>
            /// Domain must not depend on outer layers.
            /// </summary>
            [Fact]
            public void Domain_Should_Not_Depend_On_Other_Layers()
            {
                bool result = Types.InAssembly(DomainAssembly)
                    .Should()
                    .NotHaveDependencyOn("{{name}}.Application")
                    .And()
                    .NotHaveDependencyOn("{{name}}.Infrastructure")
                    .And()
                    .NotHaveDependencyOn("{{name}}.Web")
                    .GetResult()
                    .IsSuccessful;

                Assert.True(result);
            }

            /// <summary>
            /// Application must not depend on Web.
            /// </summary>
            [Fact]
            public void Application_Should_Not_Depend_On_Web()
            {
                bool result = Types.InAssembly(ApplicationAssembly)
                    .Should()
                    .NotHaveDependencyOn("{{name}}.Web")
                    .GetResult()
                    .IsSuccessful;

                Assert.True(result);
            }

            /// <summary>
            /// Infrastructure must not depend on Web.
            /// </summary>
            [Fact]
            public void Infrastructure_Should_Not_Depend_On_Web()
            {
                bool result = Types.InAssembly(InfrastructureAssembly)
                    .Should()
                    .NotHaveDependencyOn("{{name}}.Web")
                    .GetResult()
                    .IsSuccessful;

                Assert.True(result);
            }
        }
        """;
    }

    /// <summary>
    /// Returns content of generated observability rules test.
    /// </summary>
    private static string GetObservabilityRulesTestsContent(string name, string owner, string license)
    {
        return GetFileHeader("ObservabilityRulesTests.cs", owner, license) + $$"""
        namespace {{name}}.ArchitectureTests;

        using System;
        using System.IO;
        using System.Linq;
        using System.Reflection;
        using Xunit;

        /// <summary>
        /// Enforces mandatory BaseDDD observability rules.
        /// </summary>
        public sealed class ObservabilityRulesTests
        {
            /// <summary>
            /// Ensures Observability namespace exists.
            /// </summary>
            [Fact]
            public void Observability_Namespace_Must_Exist()
            {
                Assembly webAssembly = Assembly.Load("{{name}}.Web");

                bool exists = webAssembly
                    .GetTypes()
                    .Any(t => t.Namespace != null &&
                            t.Namespace.Contains(".Web.Observability"));

                Assert.True(exists);
            }

            /// <summary>
            /// Ensures Program.cs configures observability.
            /// </summary>
            [Fact]
            public void Program_Must_Configure_Observability()
            {
                string baseDirectory = AppContext.BaseDirectory;

                DirectoryInfo? directory = new DirectoryInfo(baseDirectory);

                while (directory != null && !Directory.Exists(Path.Combine(directory.FullName, "src")))
                {
                    directory = directory.Parent;
                }

                if (directory is null)
                {
                    throw new InvalidOperationException("Could not locate solution root.");
                }

                string programPath = Path.Combine(
                    directory.FullName,
                    "src",
                    "{{name}}.Web",
                    "Program.cs");

                Assert.True(File.Exists(programPath));

                string content = File.ReadAllText(programPath);

                Assert.Contains("AddBaseDDDObservability", content);
                Assert.Contains("UseBaseDDDObservability", content);
                Assert.Contains("EnsureBaseDDDCompliance", content);
            }
        }
        """;
    }

    /// <summary>
    /// Returns content of initial integration test.
    /// </summary>
    private static string GetInitialIntegrationTestContent(string name, string owner, string license)
    {
        return GetFileHeader("InitialIntegrationTests.cs", owner, license) + $$"""
        namespace {{name}}.IntegrationTests;

        using Xunit;

        /// <summary>
        /// Basic integration test placeholder.
        /// </summary>
        public sealed class InitialIntegrationTests
        {
            /// <summary>
            /// Ensures test project is working.
            /// </summary>
            [Fact]
            public void Should_Pass()
            {
                Assert.True(true);
            }
        }
        """;
    }

    /// <summary>
    /// Returns content of generated correlation middleware.
    /// </summary>
    private static string GetCorrelationMiddlewareContent(string name, string owner, string license)
    {
        return GetFileHeader("CorrelationMiddleware.cs", owner, license) + $$"""
        namespace {{name}}.Web.Observability;

        using System;
        using System.Diagnostics;
        using System.Threading.Tasks;
        using Microsoft.AspNetCore.Http;

        /// <summary>
        /// Middleware responsible for injecting correlation id into requests.
        /// </summary>
        public sealed class CorrelationMiddleware
        {
            private const string HeaderName = "X-Correlation-Id";

            private readonly RequestDelegate next;

            /// <summary>
            /// Initializes a new instance of the <see cref="CorrelationMiddleware"/> class.
            /// </summary>
            /// <param name="next">Next delegate.</param>
            public CorrelationMiddleware(RequestDelegate next)
            {
                this.next = next;
            }

            /// <summary>
            /// Executes middleware logic.
            /// </summary>
            /// <param name="context">HTTP context.</param>
            /// <returns>Task.</returns>
            public async Task InvokeAsync(HttpContext context)
            {
                string correlationId;

                if (!context.Request.Headers.TryGetValue(HeaderName, out Microsoft.Extensions.Primitives.StringValues headerValue))
                {
                    correlationId = Guid.NewGuid().ToString();
                    context.Request.Headers[HeaderName] = correlationId;
                }
                else
                {
                    correlationId = headerValue.ToString();
                }

                context.Response.Headers[HeaderName] = correlationId;

                using Activity activity = new Activity("BaseDDD.Request");
                activity.SetIdFormat(ActivityIdFormat.W3C);
                activity.Start();
                activity.SetTag("correlation.id", correlationId);

                await this.next(context);
            }
        }
        """;
    }

    /// <summary>
    /// Returns content of generated observability extensions.
    /// </summary>
    private static string GetObservabilityExtensionsContent(string name, string owner, string license)
    {
        return GetFileHeader("ObservabilityExtensions.cs", owner, license) + $$"""
        namespace {{name}}.Web.Observability;

        using System;
        using Microsoft.AspNetCore.Builder;
        using Microsoft.Extensions.DependencyInjection;
        using OpenTelemetry.Trace;
        using Serilog;

        /// <summary>
        /// Provides observability configuration extensions.
        /// </summary>
        public static class ObservabilityExtensions
        {
            private static bool configured;
            private static bool middlewareApplied;

            /// <summary>
            /// Adds mandatory BaseDDD observability configuration.
            /// </summary>
            /// <param name="services">Service collection.</param>
            /// <returns>Updated service collection.</returns>
            public static IServiceCollection AddBaseDDDObservability(this IServiceCollection services)
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();

                services.AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddSerilog();
                });

                services.AddOpenTelemetry()
                    .WithTracing(builder =>
                    {
                        builder
                            .AddAspNetCoreInstrumentation()
                            .AddConsoleExporter();
                    });

                configured = true;

                return services;
            }

            /// <summary>
            /// Enables mandatory middleware pipeline.
            /// </summary>
            /// <param name="app">Application builder.</param>
            /// <returns>Updated application builder.</returns>
            public static IApplicationBuilder UseBaseDDDObservability(this IApplicationBuilder app)
            {
                if (!configured)
                {
                    throw new InvalidOperationException(
                        "BaseDDD Observability not configured. Call AddBaseDDDObservability().");
                }

                app.UseMiddleware<CorrelationMiddleware>();

                middlewareApplied = true;

                return app;
            }

            /// <summary>
            /// Verifies that BaseDDD observability was fully configured.
            /// </summary>
            public static void EnsureBaseDDDCompliance()
            {
                if (!configured || !middlewareApplied)
                {
                    throw new InvalidOperationException(
                        "BaseDDD observability not fully configured. " +
                        "Ensure AddBaseDDDObservability() and UseBaseDDDObservability() are called.");
                }
            }
        }
        """;
    }

    /// <summary>
    /// Returns content of generated Program.cs.
    /// </summary>
    private static string GetProgramFileContent(string name, string owner, string license)
    {
        return GetFileHeader("Program.cs", owner, license) + $$"""
        namespace {{name}}.Web;

        using Microsoft.AspNetCore.Builder;
        using Microsoft.Extensions.DependencyInjection;
        using Microsoft.Extensions.Hosting;
        using {{name}}.Web.Observability;

        /// <summary>
        /// Entry point.
        /// </summary>
        public static class Program
        {
            /// <summary>
            /// Main method.
            /// </summary>
            /// <param name="args">Arguments.</param>
            public static void Main(string[] args)
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                builder.Services.AddControllers();
                builder.Services.AddBaseDDDObservability();

                WebApplication app = builder.Build();

                app.UseBaseDDDObservability();
                app.UseAuthorization();
                app.MapControllers();
                ObservabilityExtensions.EnsureBaseDDDCompliance();

                app.Run();
            }
        }
        """;
    }

    /// <summary>
    /// Returns the generated Header for a file.
    /// </summary>
    private static string GetFileHeader(string fileName, string owner, string license)
    {
        int year = DateTime.UtcNow.Year;

        return $$"""
        // <copyright file="{{fileName}}" company="{{owner}}">
        // Copyright (c) {{year}} {{owner}}. Licensed under the {{license}} License.
        // </copyright>

        """;
    }

    /// <summary>
    /// Handles the 'lint' command.
    /// </summary>
    private static void HandleLint()
    {
        string root = Directory.GetCurrentDirectory();

        ValidateDirectory(root, "src");
        ValidateDirectory(root, "tests");
        ValidateDirectory(root, "docker");

        ValidateFile(root, "Directory.Build.props");
        ValidateFile(root, "stylecop.json");
    }

    /// <summary>
    /// Checks if "name" directory exists inside "root".
    /// </summary>
    private static void ValidateDirectory(string root, string name)
    {
        if (!Directory.Exists(Path.Combine(root, name)))
        {
            throw new InvalidOperationException($"{name} folder missing.");
        }
    }

    /// <summary>
    /// Checks if "name" file exists inside "root".
    /// </summary>
    private static void ValidateFile(string root, string name)
    {
        if (!File.Exists(Path.Combine(root, name)))
        {
            throw new InvalidOperationException($"{name} file missing.");
        }
    }

    /// <summary>
    /// Handles the 'verify' command.
    /// </summary>
    private static void HandleVerify()
    {
        HandleLint();
        RunDotnet("build", Directory.GetCurrentDirectory());
        RunDotnet("test", Directory.GetCurrentDirectory());
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

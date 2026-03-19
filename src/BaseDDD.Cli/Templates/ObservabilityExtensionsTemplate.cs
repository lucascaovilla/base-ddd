// <copyright file="ObservabilityExtensionsTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides ObservabilityExtensions file template.
/// </summary>
public static class ObservabilityExtensionsTemplate
{
    /// <summary>
    /// Returns content of generated observability extensions.
    /// </summary>
    /// <param name="name">Repository name.</param>
    /// <param name="owner">Repository owner.</param>
    /// <param name="license">Repository license.</param>
    /// <returns>ObservabilityExtensions file content.</returns>
    public static string Generate(string name, string owner, string license)
    {
        return FileHeaderTemplate.Generate("ObservabilityExtensions.cs", owner, license) + $$"""
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
}

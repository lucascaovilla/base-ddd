// <copyright file="InitialIntegrationTestTemplate.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Templates;

/// <summary>
/// Provides InitialIntegrationTest.cs file template.
/// </summary>
public static class InitialIntegrationTestTemplate
{
    /// <summary>
    /// Returns content of initial integration test.
    /// </summary>
    /// <param name="name">Repository name.</param>
    /// <param name="owner">Repository owner.</param>
    /// <param name="license">Repository license.</param>
    /// <returns>InitialIntegrationTest.cs file content.</returns>
    public static string Generate(string name, string owner, string license)
    {
        return FileHeaderTemplate.Generate("InitialIntegrationTests.cs", owner, license) + $$"""
        namespace {{name}}.IntegrationTests;

        using Xunit;

        /// <summary>
        /// Basic integration test placeholder.
        /// </summary>
        public class InitialIntegrationTests
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
}

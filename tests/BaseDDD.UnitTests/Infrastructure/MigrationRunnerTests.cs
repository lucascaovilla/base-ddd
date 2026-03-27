// <copyright file="MigrationRunnerTests.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.UnitTests.Infrastructure;

using System;
using System.IO;
using BaseDDD.Infrastructure;
using Xunit;

public class MigrationRunnerTests
{
    [Fact]
    public void DryRun_Should_Not_Throw_When_Already_At_Current_Version()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string current = VersionConstants.TemplateVersion;

        MigrationRunner runner = MigrationRunner.Create();

        Exception exception = Record.Exception(
            () => runner.DryRun(root, current, current));

        Assert.Null(exception);
    }

    [Fact]
    public void Apply_Should_Not_Throw_When_Already_At_Current_Version()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string current = VersionConstants.TemplateVersion;

        MigrationRunner runner = MigrationRunner.Create();

        Exception exception = Record.Exception(
            () => runner.Apply(root, current, current));

        Assert.Null(exception);
    }

    [Fact]
    public void Apply_Should_Throw_When_Chain_Is_Broken()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        MigrationRunner runner = MigrationRunner.Create();

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => runner.Apply(root, "0.1", VersionConstants.TemplateVersion));

        Assert.Contains("Migration chain is broken", exception.Message);
    }

    [Fact]
    public void DryRun_Should_Throw_When_Chain_Is_Broken()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        MigrationRunner runner = MigrationRunner.Create();

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => runner.DryRun(root, "0.1", VersionConstants.TemplateVersion));

        Assert.Contains("Migration chain is broken", exception.Message);
    }
}

// <copyright file="VersionEnforcementGeneratorTests.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.UnitTests.Generation;

using System;
using System.IO;
using BaseDDD.Generation;
using Xunit;

public class VersionEnforcementGeneratorTests
{
    [Fact]
    public void Generate_Should_Create_BaseDddJson()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);

        new VersionEnforcementGenerator(root).Generate();

        Assert.True(File.Exists(Path.Combine(root, "baseddd.json")));
    }

    [Fact]
    public void Generate_Should_Write_Valid_Json()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);

        new VersionEnforcementGenerator(root).Generate();

        string content = File.ReadAllText(Path.Combine(root, "baseddd.json"));
        Templates.Helpers.TemplateValidationHelper.ValidateJson(content);
    }

    [Fact]
    public void Generate_Should_Throw_When_BaseDddJson_Already_Exists()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, "baseddd.json"), "{}");

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => new VersionEnforcementGenerator(root).Generate());

        Assert.Contains("baseddd migrate", exception.Message);
    }

    [Fact]
    public void Generate_Should_Set_CreatedAt_Equal_To_UpdatedAt()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);

        new VersionEnforcementGenerator(root).Generate();

        string content = File.ReadAllText(Path.Combine(root, "baseddd.json"));
        using System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(content);
        string createdAt = doc.RootElement.GetProperty("createdAt").GetString()!;
        string updatedAt = doc.RootElement.GetProperty("updatedAt").GetString()!;

        Assert.Equal(createdAt, updatedAt);
    }

    [Fact]
    public void Migrate_Should_Throw_When_BaseDddJson_Is_Missing()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => new VersionEnforcementGenerator(root).Migrate(dryRun: false));

        Assert.Contains("baseddd.json not found", exception.Message);
    }

    [Fact]
    public void Migrate_DryRun_Should_Not_Modify_File()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);
        new VersionEnforcementGenerator(root).Generate();

        string before = File.ReadAllText(Path.Combine(root, "baseddd.json"));

        new VersionEnforcementGenerator(root).Migrate(dryRun: true);

        string after = File.ReadAllText(Path.Combine(root, "baseddd.json"));
        Assert.Equal(before, after);
    }

    [Fact]
    public void Migrate_Should_Preserve_CreatedAt()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);
        new VersionEnforcementGenerator(root).Generate();

        string before = File.ReadAllText(Path.Combine(root, "baseddd.json"));
        using System.Text.Json.JsonDocument docBefore = System.Text.Json.JsonDocument.Parse(before);
        string createdAtBefore = docBefore.RootElement.GetProperty("createdAt").GetString()!;

        new VersionEnforcementGenerator(root).Migrate(dryRun: false);

        string after = File.ReadAllText(Path.Combine(root, "baseddd.json"));
        using System.Text.Json.JsonDocument docAfter = System.Text.Json.JsonDocument.Parse(after);
        string createdAtAfter = docAfter.RootElement.GetProperty("createdAt").GetString()!;

        Assert.Equal(createdAtBefore, createdAtAfter);
    }

    [Fact]
    public void Check_Should_Not_Throw_When_BaseDddJson_Is_Missing()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);

        Exception exception = Record.Exception(
            () => new VersionEnforcementGenerator(root).Check());

        Assert.Null(exception);
    }

    [Fact]
    public void Check_Should_Not_Throw_When_BaseDddJson_Is_Current()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);
        new VersionEnforcementGenerator(root).Generate();

        Exception exception = Record.Exception(
            () => new VersionEnforcementGenerator(root).Check());

        Assert.Null(exception);
    }
}

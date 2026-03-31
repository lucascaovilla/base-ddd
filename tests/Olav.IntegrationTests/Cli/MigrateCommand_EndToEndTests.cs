// <copyright file="MigrateCommand_EndToEndTests.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.IntegrationTests.Cli;

using System.IO;
using Olav.IntegrationTests.Generation.Fixtures;
using Xunit;

[Collection("GeneratedProject")]
public class MigrateCommand_EndToEndTests(GeneratedProjectFixture fixture)
{
    private readonly GeneratedProjectFixture fixture = fixture;

    [Fact]
    public void Should_Have_OlavJson_After_New()
    {
        Assert.True(File.Exists(Path.Combine(this.fixture.ProjectPath, "olav.json")));
    }

    [Fact]
    public void OlavJson_Should_Contain_Expected_Fields()
    {
        string content = File.ReadAllText(Path.Combine(this.fixture.ProjectPath, "olav.json"));
        using System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(content);
        System.Text.Json.JsonElement root = doc.RootElement;

        Assert.True(root.TryGetProperty("toolVersion", out _));
        Assert.True(root.TryGetProperty("templateVersion", out _));
        Assert.True(root.TryGetProperty("createdAt", out _));
        Assert.True(root.TryGetProperty("updatedAt", out _));
    }

    [Fact]
    public void OlavJson_Should_Contain_Current_TemplateVersion()
    {
        string content = File.ReadAllText(Path.Combine(this.fixture.ProjectPath, "olav.json"));
        using System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(content);
        string templateVersion = doc.RootElement.GetProperty("templateVersion").GetString()!;

        Assert.Equal(VersionConstants.TemplateVersion, templateVersion);
    }
}

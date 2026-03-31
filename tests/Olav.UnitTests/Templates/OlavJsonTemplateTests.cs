// <copyright file="OlavJsonTemplateTests.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.UnitTests.Templates;

using System;
using Olav.Templates;
using Olav.UnitTests.Templates.Helpers;
using Xunit;

public class OlavJsonTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Json()
    {
        string content = OlavJsonTemplate.Generate(
            "1.0.0",
            "1.0",
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc));

        TemplateValidationHelper.ValidateJson(content);
    }

    [Fact]
    public void Should_Contain_ToolVersion()
    {
        string content = OlavJsonTemplate.Generate(
            "1.2.3",
            "1.0",
            DateTime.UtcNow,
            DateTime.UtcNow);

        Assert.Contains("\"toolVersion\": \"1.2.3\"", content);
    }

    [Fact]
    public void Should_Contain_TemplateVersion()
    {
        string content = OlavJsonTemplate.Generate(
            "1.0.0",
            "1.0",
            DateTime.UtcNow,
            DateTime.UtcNow);

        Assert.Contains("\"templateVersion\": \"1.0\"", content);
    }

    [Fact]
    public void Should_Contain_CreatedAt_In_Iso8601()
    {
        DateTime createdAt = new(2026, 3, 15, 10, 30, 0, DateTimeKind.Utc);

        string content = OlavJsonTemplate.Generate(
            "1.0.0",
            "1.0",
            createdAt,
            DateTime.UtcNow);

        Assert.Contains("\"createdAt\": \"2026-03-15T10:30:00Z\"", content);
    }

    [Fact]
    public void Should_Contain_UpdatedAt_In_Iso8601()
    {
        DateTime updatedAt = new(2026, 6, 20, 8, 0, 0, DateTimeKind.Utc);

        string content = OlavJsonTemplate.Generate(
            "1.0.0",
            "1.0",
            DateTime.UtcNow,
            updatedAt);

        Assert.Contains("\"updatedAt\": \"2026-06-20T08:00:00Z\"", content);
    }

    [Fact]
    public void Should_Preserve_CreatedAt_Different_From_UpdatedAt()
    {
        DateTime createdAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime updatedAt = new(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc);

        string content = OlavJsonTemplate.Generate("1.0.0", "1.0", createdAt, updatedAt);

        Assert.Contains("2026-01-01T00:00:00Z", content);
        Assert.Contains("2026-06-01T00:00:00Z", content);
    }
}

using Xunit;
using System;

namespace BaseDDD.UnitTests.Templates;

public class DockerfileTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Dockerfile()
    {
        string content = BaseDDD.Templates.DockerfileTemplate.Generate("Test");

        Assert.Contains("FROM", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("WORKDIR", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("COPY", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("RUN", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("dotnet publish", content, StringComparison.OrdinalIgnoreCase);
    }
}

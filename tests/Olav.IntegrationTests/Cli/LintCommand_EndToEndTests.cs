using System;
using System.IO;
using Xunit;
using Olav.IntegrationTests.Generation.Fixtures;

namespace Olav.IntegrationTests.Cli;

[Collection("GeneratedProject")]
public class LintCommand_EndToEndTests(GeneratedProjectFixture fixture)
{
    private readonly GeneratedProjectFixture fixture = fixture;

    [Fact]
    public void Should_Run_Without_Error()
    {
        Directory.SetCurrentDirectory(this.fixture.ProjectPath);

        Exception ex = Record.Exception(() =>
            Program.Main(["lint"])
        );

        Assert.Null(ex);
    }

    [Fact]
    public void Should_Pass_When_OlavJson_Is_Present_And_Current()
    {
        Directory.SetCurrentDirectory(this.fixture.ProjectPath);

        Exception exception = Record.Exception(() =>
            Program.Main(["lint"]));

        Assert.Null(exception);
    }
}

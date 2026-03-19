using System;
using System.IO;
using Xunit;
using BaseDDD.IntegrationTests.Generation.Fixtures;

namespace BaseDDD.IntegrationTests.Cli;

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
}

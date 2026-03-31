using System;
using System.IO;
using Xunit;
using Olav.IntegrationTests.Generation.Fixtures;

namespace Olav.IntegrationTests.Cli;

[Collection("GeneratedProject")]
public class VerifyCommand_EndToEndTests(GeneratedProjectFixture fixture)
{
    private readonly GeneratedProjectFixture _fixture = fixture;

    [Fact]
    public void Should_Run_Without_Error()
    {
        Directory.SetCurrentDirectory(this._fixture.ProjectPath);

        Exception ex = Record.Exception(() =>
            Program.Main(["verify"])
        );

        Assert.Null(ex);
    }
}

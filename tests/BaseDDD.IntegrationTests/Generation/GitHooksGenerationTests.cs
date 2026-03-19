using System.IO;
using Xunit;
using BaseDDD.IntegrationTests.Generation.Fixtures;

namespace BaseDDD.IntegrationTests.Generation;

[Collection("GeneratedProject")]
public class GitHooksGenerationTests
{
    private readonly GeneratedProjectFixture _fixture;

    public GitHooksGenerationTests(GeneratedProjectFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void Should_Create_GitHooks()
    {
        string hooksPath = Path.Combine(this._fixture.ProjectPath, ".githooks");

        Assert.True(File.Exists(Path.Combine(hooksPath, "pre-commit")));
        Assert.True(File.Exists(Path.Combine(hooksPath, "pre-push")));
    }
}

using System.IO;
using Xunit;
using BaseDDD.IntegrationTests.Generation.Fixtures;

namespace BaseDDD.IntegrationTests.Generation;

[Collection("GeneratedProject")]
public class DockerFilesGenerationTests
{
    private readonly GeneratedProjectFixture _fixture;

    public DockerFilesGenerationTests(GeneratedProjectFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void Should_Generate_Docker_Files()
    {
        string dockerPath = Path.Combine(this._fixture.ProjectPath, "docker");

        Assert.True(File.Exists(Path.Combine(dockerPath, "Dockerfile")));
        Assert.True(File.Exists(Path.Combine(dockerPath, "docker-compose.yml")));
    }
}
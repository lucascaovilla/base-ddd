using System.IO;
using Xunit;
using BaseDDD.IntegrationTests.Generation.Fixtures;
using BaseDDD.Infrastructure;

namespace BaseDDD.IntegrationTests.Cli;

[Collection("GeneratedProject")]
public class Docker_Compose_EndToEndTests(GeneratedProjectFixture fixture)
{
    private readonly GeneratedProjectFixture _fixture = fixture;

    [Fact]
    public void Docker_Compose_Should_Build_Or_Validate_Successfully()
    {
        string projectPath = this._fixture.ProjectPath;
        string dockerPath = Path.Combine(projectPath, "docker");

        string dockerfilePath = Path.Combine(dockerPath, "Dockerfile");
        Assert.True(File.Exists(dockerfilePath));

        string[] buildFiles = { "docker-compose.dev.yml", "docker-compose.local.yml" };
        foreach (string f in buildFiles)
        {
            ProcessRunner.Run("docker", $"compose -f {f} build --no-cache", dockerPath);
        }

        string[] validateOnlyFiles = ["docker-compose.yml", "docker-compose.staging.yml"];
        foreach (string f in validateOnlyFiles)
        {
            ProcessRunner.Run("docker", $"compose -f {f} config", dockerPath);
        }
    }
}

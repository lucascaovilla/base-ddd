using System.IO;
using Xunit;
using Olav.IntegrationTests.Generation.Fixtures;

namespace Olav.IntegrationTests.Generation;

[Collection("GeneratedProject")]
public class GeneratedProjectStructureTests
{
    private readonly GeneratedProjectFixture _fixture;

    public GeneratedProjectStructureTests(GeneratedProjectFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void Should_Generate_Expected_Folders_And_Files()
    {
        string projectPath = Path.Combine(this._fixture.ProjectPath);

        Assert.True(File.Exists(Path.Combine(projectPath, $"{this._fixture.ProjectName}.slnx")));
        Assert.True(Directory.Exists(Path.Combine(projectPath, "src")));
        Assert.True(Directory.Exists(Path.Combine(projectPath, "tests")));
    }
}

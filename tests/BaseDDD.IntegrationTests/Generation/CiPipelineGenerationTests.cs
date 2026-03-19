using System.IO;
using Xunit;
using BaseDDD.IntegrationTests.Generation.Fixtures;

namespace BaseDDD.IntegrationTests.Generation;

[Collection("GeneratedProject")]
public class CiPipelineGenerationTests
{
    private readonly GeneratedProjectFixture _fixture;

    public CiPipelineGenerationTests(GeneratedProjectFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void Should_Generate_CI_Workflow()
    {
        string ciPath = Path.Combine(this._fixture.ProjectPath, ".github/workflows");

        Assert.True(File.Exists(Path.Combine(ciPath, "ci.yml")));
    }
}

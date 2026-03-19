using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class DockerComposeTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Compose()
    {
        string contentPrd = BaseDDD.Templates.DockerComposeTemplate.GeneratePrd("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentPrd);

        string contentStaging = BaseDDD.Templates.DockerComposeTemplate.GenerateStaging("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentStaging);

        string contentDev = BaseDDD.Templates.DockerComposeTemplate.GenerateDev("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentDev);

        string contentLocal = BaseDDD.Templates.DockerComposeTemplate.GenerateLocal("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentLocal);

    }
}

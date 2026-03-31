using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class DockerComposeTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Compose()
    {
        string contentPrd = Olav.Templates.DockerComposeTemplate.GeneratePrd("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentPrd);

        string contentStaging = Olav.Templates.DockerComposeTemplate.GenerateStaging("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentStaging);

        string contentDev = Olav.Templates.DockerComposeTemplate.GenerateDev("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentDev);

        string contentLocal = Olav.Templates.DockerComposeTemplate.GenerateLocal("Test");
        TemplateValidationHelper.ValidateDockerCompose(contentLocal);

    }
}

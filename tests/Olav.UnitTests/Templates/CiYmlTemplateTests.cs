using Xunit;
using Olav.UnitTests.Templates.Helpers;
using Olav.Templates;

namespace Olav.UnitTests.Templates;

public class CiYmlTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Yaml()
    {
        string content = CiYmlTemplate.Generate();

        TemplateValidationHelper.ValidateYaml(content);
    }
}

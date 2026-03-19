using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;
using BaseDDD.Templates;

namespace BaseDDD.UnitTests.Templates;

public class CiYmlTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Yaml()
    {
        string content = CiYmlTemplate.Generate();

        TemplateValidationHelper.ValidateYaml(content);
    }
}
